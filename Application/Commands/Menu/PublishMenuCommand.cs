using Application.Interfaces;
using Contracts.Repositories;
using Domain.Exceptions;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Menu;

public record PublishMenuCommand(Guid KitchenId, Guid MenuId) : IRequest, IKitchenRequest;

internal sealed class PublishMenuHandler : IRequestHandler<PublishMenuCommand>
{
    private readonly ILogger<PublishMenuHandler> _logger;
    private readonly IRepositoryManager _repository;
    private readonly EventsDispatcher _eventsDispatcher;

    public async Task Handle(PublishMenuCommand request, CancellationToken cancellationToken)
    {
        var kitchen = await _repository.Kitchens.GetKitchenAsync(request.KitchenId);
        var menu = await _repository.Menu.GetMenuAsync(request.MenuId);
        
        if (menu.KitchenId != kitchen.Id)
            throw new NotFoundException("Меню {MenuId} не найдено для кухни {KitchenId}", request.MenuId, request.KitchenId);

        menu.Publish();
        
        _repository.Menu.UpdateMenu(menu);
        await _repository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("Меню {MenuId} опубликовано", menu.Id);

        await _eventsDispatcher.DispatchDomainEvents(menu);
    }

    public PublishMenuHandler(IRepositoryManager repository, ILogger<PublishMenuHandler> logger, EventsDispatcher eventsDispatcher)
    {
        _repository = repository;
        _logger = logger;
        _eventsDispatcher = eventsDispatcher;
    }
}