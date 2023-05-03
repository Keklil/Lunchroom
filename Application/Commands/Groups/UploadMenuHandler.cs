using System.Text.Encodings.Web;
using System.Text.Json;
using Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Groups;

public sealed record UploadMenuFromFile(Guid KitchenId, IFormFile Menu) : INotification;

internal sealed class UploadMenuFromFileHandler : INotificationHandler<UploadMenuFromFile>
{
    private readonly ILogger<UploadMenuFromFileHandler> _logger;
    private readonly IMenuImportService _menuImportService;
    private readonly ISender _sender;

    public async Task Handle(UploadMenuFromFile command, CancellationToken token)
    {
        var report = await _menuImportService.ImportMenuAsync(command.KitchenId, command.Menu);
    }

    public UploadMenuFromFileHandler(ILogger<UploadMenuFromFileHandler> logger, ISender sender, IMenuImportService menuImportService)
    {
        _logger = logger;
        _sender = sender;
        _menuImportService = menuImportService;
    }
}