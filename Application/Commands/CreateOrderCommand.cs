using Contracts.Repositories;
using Domain.Models;
using MediatR;
using Shared.DataTransferObjects.Order;

namespace Application.Commands;

public sealed record CreateOrderCommand(OrderForCreationDto Order) : IRequest<OrderDto>;

internal sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IRepositoryManager _repository;

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = new Order(request.Order.CustomerId, request.Order.MenuId, request.Order.GroupId);

        var menu = await _repository.Menu.GetMenuAsync(request.Order.MenuId, false);

        if (request.Order.LunchSetId != default)
        {
            var lunchSet = menu.GetLunchSetById(request.Order.LunchSetId);
            orderEntity.AddLunchSet(lunchSet, request.Order.LunchSetUnits);
        }

        var orderOptions = request.Order.Options;

        foreach (var item in orderOptions)
        {
            var option = menu.Options.SingleOrDefault(x => x.Id == item.OptionId);
            orderEntity.AddOption(option, item.Units);
        }

        orderEntity.ChangeStatus(1);

        _repository.Order.CreateOrder(orderEntity);
        await _repository.SaveAsync();

        var orderToReturn = orderEntity.Map();

        return orderToReturn;
    }

    public CreateOrderHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}