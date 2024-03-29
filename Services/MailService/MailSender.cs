﻿using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Contracts;
using MailKit.Security;

namespace Services.MailService;

public class MailSender : IMailSender
{
    private readonly IConfiguration _configuration;

    private string serviceAccount, password, domainName, host;
    private int port;
    
    public MailSender(IConfiguration configuration)
    {
        _configuration = configuration;
        
        serviceAccount = _configuration.GetValue<string>("MailServer:ServiceAccount");
        password = _configuration.GetValue<string>("MailServer:Password");
        domainName = _configuration.GetValue<string>("MailServer:DomainName");
        host = _configuration.GetValue<string>("MailServer:HostSmtp");
        port = _configuration.GetValue<int>("MailServer:PortSmtp");

    }
    
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
 
        emailMessage.From.Add(new MailboxAddress("Lunchroom", serviceAccount + "@yandex.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(host, port);
            await client.AuthenticateAsync(serviceAccount, password);
            await client.SendAsync(emailMessage);
 
            await client.DisconnectAsync(true);
        }
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