using Entities.DataTransferObjects;
using MediatR;
using AutoMapper;
using Entities.Models;
using Contracts;
using Entities.Exceptions;

namespace Application.Queries;

public sealed record GetOrderQuery(Guid OrderId) : IRequest<OrderDto>;

internal class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    
    public GetOrderHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.Order.GetOrderAsync(request.OrderId, false);
        if (orderEntity is null)
            throw new NotFoundException("Order not found");

        var menu = await _repository.Menu.GetMenuAsync(orderEntity.MenuId, false);
        
        
        var order = _mapper.Map<OrderDto>(orderEntity);
        
        var lunchSet = menu.LunchSets
            .Where(x => x.Id == orderEntity.LunchSetId)
            .SingleOrDefault();

        order.LunchSet = _mapper.Map<LunchSetDto>(lunchSet);
        foreach (var option in order.Options)
        {
            var optionFromMenu = menu.Options
                .Where(x => x.Id == option.OptionId)
                .SingleOrDefault();

            option.Option = _mapper.Map<OptionDto>(optionFromMenu);
        }
        
        return order;
    }
}