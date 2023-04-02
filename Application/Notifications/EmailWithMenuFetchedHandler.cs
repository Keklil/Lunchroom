﻿using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Commands;
using Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notifications;

public sealed record EmailWithMenuFetched(string MailBody, Guid GroupId) : INotification;

internal sealed class EmailWithMenuFetchedHandler : INotificationHandler<EmailWithMenuFetched>
{
    private readonly ILogger<EmailWithMenuFetchedHandler> _logger;
    private readonly IMailParser _mailParser;
    private readonly ISender _sender;

    public async Task Handle(EmailWithMenuFetched notification, CancellationToken token)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        var mailBody = notification.MailBody;
        var menuRaw = _mailParser.NormalizeMenu(mailBody);
        var text = JsonSerializer.Serialize(menuRaw, options);
        _logger.LogInformation("Меню нормализовано: {NormalizedMenu}", text);

        var menuDto = _mailParser.ConvertMenu(menuRaw);
        text = JsonSerializer.Serialize(menuDto, options);
        _logger.LogInformation("Меню считано: {ParsedMenu}", text);

        var menu = await _sender.Send(new CreateMenuCommand(menuDto, notification.GroupId));
    }

    public EmailWithMenuFetchedHandler(ILogger<EmailWithMenuFetchedHandler> logger, ISender sender, IMailParser mailService)
    {
        _logger = logger;
        _sender = sender;
        _mailParser = mailService;
    }
}