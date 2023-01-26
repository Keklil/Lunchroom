using Application.Commands;
using Contracts;
using MediatR;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Application.Notifications
{
    public sealed record EmailWithMenuFetched(string MailBody, Guid GroupId) : INotification;

    internal sealed class EmailWithMenuFetchedHandler : INotificationHandler<EmailWithMenuFetched>
    {
        private readonly ILoggerManager _logger;
        private readonly ISender _sender;
        private readonly IMailParser _mailParser;

        public EmailWithMenuFetchedHandler(ILoggerManager logger, ISender sender, IMailParser mailService)
        {
            _logger = logger;
            _sender = sender;
            _mailParser = mailService;
        }

        public async Task Handle(EmailWithMenuFetched notification, CancellationToken token)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            var mailBody = notification.MailBody;
            List<string> menuRaw = _mailParser.NormalizeMenu(mailBody);
            var text = JsonSerializer.Serialize(menuRaw, options);
            _logger.LogInfo($"{text}");
            
            var menuDto = _mailParser.ConvertMenu(menuRaw);
            text = JsonSerializer.Serialize(menuDto, options);
            _logger.LogInfo($"{text}");
            
            var menu = await _sender.Send(new CreateMenuCommand(menuDto, notification.GroupId));
        }
    }

}
