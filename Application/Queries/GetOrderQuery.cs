using AutoMapper;
using Contracts.Repositories;
using Domain.DataTransferObjects.Menu;
using Domain.DataTransferObjects.Order;
using Domain.Exceptions;
using MediatR;

namespace Application.Queries;

public sealed record GetOrderQuery(Guid OrderId) : IRequest<OrderDto>;

internal class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.Order.GetOrderAsync(request.OrderId, false);
        if (orderEntity is null)
            throw new NotFoundException("Order not found");

        var menu = await _repository.Menu.GetMenuAsync(orderEntity.MenuId, false);

        var order = _mapper.Map<OrderDto>(orderEntity);

        if (orderEntity.LunchSetId != default)
        {
            var lunchSet = menu.LunchSets
                .Where(x => x.Id == orderEntity.LunchSetId)
                .SingleOrDefault();

            order.LunchSet = _mapper.Map<LunchSetDto>(lunchSet);
            order.LunchSet.LunchSetUnits = orderEntity.LunchSetUnits;
        }

        foreach (var option in order.Options)
        {
            var optionFromMenu = menu.Options
                .Where(x => x.Id == option.OptionId)
                .SingleOrDefault();

            option.Option = _mapper.Map<OptionDto>(optionFromMenu);
        }

        return order;
    }

    public GetOrderHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}