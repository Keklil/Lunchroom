using AutoMapper;
using MediatR;
using Entities.Models;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;

namespace Application.Queries;

public record GetTodayUserOrdersQuery(Guid UserId) 
    : IRequest<List<OrdersForUser>>;

internal class GetTodayUserOrdersHandler : IRequestHandler<GetTodayUserOrdersQuery, List<OrdersForUser>>
{
    private readonly IRepositoryManager _repository;
    
    public GetTodayUserOrdersHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<List<OrdersForUser>> Handle(GetTodayUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var listTodayOrder = await _repository.Order.GetTodayOrdersByUserAsync(request.UserId);

        return listTodayOrder;
    }
}