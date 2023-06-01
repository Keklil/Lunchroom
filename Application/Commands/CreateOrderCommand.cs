using Contracts.Repositories;
using Contracts.Security;
using Domain.Models;
using MediatR;
using Shared.DataTransferObjects.Order;

namespace Application.Commands;

public sealed record CreateOrderCommand(OrderForCreationDto Order) : IRequest<OrderDto>;

internal sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customerId = _currentUserService.GetUserId();
        var orderEntity = new Order(customerId, request.Order.MenuId, request.Order.GroupId);

        var menu = await _repository.Menu.GetMenuAsync(request.Order.MenuId, false);

        foreach (var orderDish in request.Order.Dishes)
        {
            orderEntity.AddDish(menu, orderDish.Id, orderDish.Quantity);
        }
        
        foreach (var orderLunchSet in request.Order.LunchSets)
        {
            var lunchSetOptions = orderLunchSet.Options.ToDictionary(x => x.Id, x => x.Quantity);
            orderEntity.AddLunchSet(menu, orderLunchSet.Id, lunchSetOptions);
        }

        _repository.Order.CreateOrder(orderEntity);
        await _repository.SaveAsync(cancellationToken);

        var orderToReturn = orderEntity.Map(menu);

        return orderToReturn;
    }

    public CreateOrderHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}