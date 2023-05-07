﻿using Application.Interfaces;
using Contracts.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Menu;

public record PublishMenuCommand(Guid KitchenId) : IRequest, IKitchenRequest;

internal sealed class PublishMenuHandler : IRequestHandler<PublishMenuCommand>
{
    private readonly ILogger<PublishMenuHandler> _logger;
    private readonly IRepositoryManager _repository;

    public async Task Handle(PublishMenuCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens.GetKitchenAsync(request.KitchenId);
        var menu = await _repository.Menu.GetMenuByDateAsync(DateTime.UtcNow, kitchen.Id);
        menu.Publish();
        _repository.Menu.UpdateMenu(menu);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("Меню {MenuId} опубликовано", menu.Id);
    }

    public PublishMenuHandler(IRepositoryManager repository, ILogger<PublishMenuHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}