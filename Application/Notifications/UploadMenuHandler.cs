using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Commands;
using Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Notifications;

public sealed record UploadMenu(string RawTextMenu, Guid GroupId) : INotification;

internal sealed class UploadMenuHandler : INotificationHandler<UploadMenu>
{
    private readonly ILogger<UploadMenuHandler> _logger;
    private readonly IPlainTextParser _plainTextParser;
    private readonly ISender _sender;

    public async Task Handle(UploadMenu notification, CancellationToken token)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };

        var mailBody = notification.RawTextMenu;
        var menuRaw = _plainTextParser.NormalizeMenu(mailBody);
        var text = JsonSerializer.Serialize(menuRaw, options);
        _logger.LogInformation("Меню нормализовано: {NormalizedMenu}", text);

        var menuDto = _plainTextParser.ConvertMenu(menuRaw);
        text = JsonSerializer.Serialize(menuDto, options);
        _logger.LogInformation("Меню считано: {ParsedMenu}", text);

        var menu = await _sender.Send(new CreateMenuCommand(menuDto, notification.GroupId), token);
    }

    public UploadMenuHandler(ILogger<UploadMenuHandler> logger, ISender sender, IPlainTextParser service)
    {
        _logger = logger;
        _sender = sender;
        _plainTextParser = service;
    }
}