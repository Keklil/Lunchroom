using Contracts;
using Contracts.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Menu;

public sealed record UploadMenuFromFile(Guid KitchenId, IFormFile Menu) : INotification;

internal sealed class UploadMenuFromFileHandler : INotificationHandler<UploadMenuFromFile>
{
    private readonly ILogger<UploadMenuFromFileHandler> _logger;
    private readonly IDataTableParser _dataTableParser;
    private readonly IRepositoryManager _repository;

    public async Task Handle(UploadMenuFromFile command, CancellationToken token)
    {
        var report = await _dataTableParser.ImportMenuAsync(command.KitchenId, command.Menu);
        _repository.Menu.CreateMenu(report.Menu);
        await _repository.SaveAsync(token);
    }

    public UploadMenuFromFileHandler(ILogger<UploadMenuFromFileHandler> logger, IDataTableParser dataTableParser, 
        IRepositoryManager repository)
    {
        _logger = logger;
        _dataTableParser = dataTableParser;
        _repository = repository;
    }
}