using Contracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Services.MailService;

public class MailSender : IMailSender
{
    private readonly int _port;
    private readonly string _serviceAccount;
    private readonly string _password;
    private readonly string _domainName;
    private readonly string _host;

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Lunchroom", _serviceAccount + "@" + _domainName));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_host, _port);
            await client.AuthenticateAsync(_serviceAccount, _password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }

    public MailSender(IConfiguration configuration)
    {
        _serviceAccount = configuration.GetValue<string>("MailServer:ServiceAccount");
        _password = configuration.GetValue<string>("MailServer:Password");
        _domainName = configuration.GetValue<string>("MailServer:DomainName");
        _host = configuration.GetValue<string>("MailServer:HostSmtp");
        _port = configuration.GetValue<int>("MailServer:PortSmtp");
    }

    /*
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
 
        emailMessage.From.Add(new MailboxAddress("Lunchroom", serviceAccount+"@yandex.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
             
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("localhost", 1025);
            //await client.AuthenticateAsync(serviceAccount, password);
            await client.SendAsync(emailMessage);
 
            await client.DisconnectAsync(true);
        }
    }*/
}