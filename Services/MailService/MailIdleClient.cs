using Application.Notifications;
using Contracts;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Services.MailService;

public class InboxIdleService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;


    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            using (var scope = _serviceProvider.CreateScope())
            {
                var idleClient = scope.ServiceProvider.GetRequiredService<IMailIdleClient>();
                try
                {
                    await idleClient.RunAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{ex}");
                }
            }
    }

    public InboxIdleService(
        ILogger logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
}

public class MailIdleClient : IMailIdleClient, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IPublisher _publisher;
    private readonly string account;
    private readonly string password;
    private readonly string host;
    private readonly CancellationTokenSource cancel;
    private readonly ImapClient client;
    private CancellationTokenSource done;
    private string mailText;

    private readonly List<IMessageSummary> messages;
    private bool messagesArrived;
    private readonly int port;
    private readonly FetchRequest request;
    private readonly string sender;

    public async Task RunAsync()
    {
        // connect to the IMAP server and get our initial list of messages
        try
        {
            await ReconnectAsync();
            await FetchMessageWithMenuAsync();
        }
        catch (OperationCanceledException)
        {
            await client.DisconnectAsync(true);
            return;
        }

        var inbox = client.Inbox;

        inbox.CountChanged += OnCountChanged;

        await IdleAsync();

        inbox.CountChanged -= OnCountChanged;

        await client.DisconnectAsync(true);
    }

    public void Exit()
    {
        cancel.Cancel();
    }

    public void Dispose()
    {
        client.Dispose();
        cancel.Dispose();
        done?.Dispose();
    }

    private async Task ReconnectAsync()
    {
        if (!client.IsConnected)
            await client.ConnectAsync(host, port, SecureSocketOptions.SslOnConnect, cancel.Token);

        if (!client.IsAuthenticated)
        {
            await client.AuthenticateAsync(account, password, cancel.Token);

            await client.Inbox.OpenAsync(FolderAccess.ReadWrite, cancel.Token);
        }

        _logger.LogInformation("Connected to email server");
    }

    private async Task IdleAsync()
    {
        do
        {
            try
            {
                await WaitForNewMessagesAsync();

                if (messagesArrived)
                {
                    await FetchMessageWithMenuAsync();
                    messagesArrived = false;
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
        } while (!cancel.IsCancellationRequested);
    }

    private async Task WaitForNewMessagesAsync()
    {
        do
        {
            try
            {
                if (client.Capabilities.HasFlag(ImapCapabilities.Idle))
                {
                    // Note: IMAP servers are only supposed to drop the connection after 30 minutes, so normally
                    // we'd IDLE for a max of, say, ~29 minutes... but GMail seems to drop idle connections after
                    // about 10 minutes, so we'll only idle for 9 minutes.
                    using (done = new CancellationTokenSource(new TimeSpan(0, 5, 0)))
                    {
                        using (var linked = CancellationTokenSource.CreateLinkedTokenSource(cancel.Token, done.Token))
                        {
                            await client.IdleAsync(linked.Token);

                            // throw OperationCanceledException if the cancel token has been canceled.
                            cancel.Token.ThrowIfCancellationRequested();
                        }
                    }
                }
                else
                {
                    // Note: we don't want to spam the IMAP server with NOOP commands, so lets wait a minute
                    // between each NOOP command.
                    await Task.Delay(new TimeSpan(0, 1, 0), cancel.Token);
                    await client.NoOpAsync(cancel.Token);
                }

                break;
            }
            catch (ImapProtocolException)
            {
                // protocol exceptions often result in the client getting disconnected
                await ReconnectAsync();
            }
            catch (IOException)
            {
                // I/O exceptions always result in the client getting disconnected
                await ReconnectAsync();
            }
        } while (true);
    }

    private async Task FetchMessageWithMenuAsync()
    {
        IList<IMessageSummary> fetched = null;

        do
        {
            try
            {
                // fetch summary information for messages that we don't already have
                var startIndex = messages.Count;

                fetched = client.Inbox.Fetch(startIndex, -1, request, cancel.Token);
                break;
            }
            catch (ImapProtocolException)
            {
                // protocol exceptions often result in the client getting disconnected
                await ReconnectAsync();
            }
            catch (IOException)
            {
                // I/O exceptions always result in the client getting disconnected
                await ReconnectAsync();
            }
        } while (true);

        var inbox = client.Inbox;

        foreach (var message in fetched)
        {
            if (message.Flags.Value.HasFlag(MessageFlags.Seen)
                || message.Date.LocalDateTime < DateTime.Today)
                continue;

            var mailboxes = message.Envelope.From.Mailboxes;

            if (mailboxes.Count() == 1)
            {
                var mailbox = mailboxes.Single().Address;
                if (mailbox == sender || mailbox == "nefedov.a.v@ft-soft.ru")
                {
                    messages.Add(message);
                    await inbox.AddFlagsAsync(message.UniqueId, MessageFlags.Seen, true, cancel.Token);
                    var mailBody = message.TextBody;
                    var body = (TextPart)client.Inbox.GetBodyPart(message.UniqueId, mailBody);
                    mailText = body.Text;
                    _logger.LogInformation($"Mail recived: {mailText}");

                    //TODO: Заглушка для групп
                    await _publisher.Publish(new UploadMenu(mailText, Guid.Empty));
                }
            }
        }
    }

    private void OnCountChanged(object? sender, EventArgs e)
    {
        var folder = (ImapFolder)sender;

        // Note: because we are keeping track of the MessageExpunged event and updating our
        // 'messages' list, we know that if we get a CountChanged event and folder.Count is
        // larger than messages.Count, then it means that new messages have arrived.
        if (folder.Count > messages.Count)
        {
            // Note: your first instinct may be to fetch these new messages now, but you cannot do
            // that in this event handler (the ImapFolder is not re-entrant).
            //
            // Instead, cancel the `done` token and update our state so that we know new messages
            // have arrived. We'll fetch the summaries for these new messages later...
            messagesArrived = true;
            done?.Cancel();
        }
    }

    public MailIdleClient(
        IConfiguration configuration,
        IPublisher publisher,
        ILogger logger)
    {
        _configuration = configuration;
        _publisher = publisher;
        _logger = logger;

        client = new ImapClient(new ProtocolLogger(Console.OpenStandardError()));
        request = new FetchRequest(MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
        messages = new List<IMessageSummary>();
        cancel = new CancellationTokenSource();
        mailText = string.Empty;

        host = _configuration.GetValue<string>("MailServer:Host");
        port = _configuration.GetValue<int>("MailServer:Port");
        account = _configuration.GetValue<string>("MailServer:ServiceAccount");
        password = _configuration.GetValue<string>("MailServer:Password");
        sender = _configuration.GetValue<string>("MailServer:Sender");
    }
}