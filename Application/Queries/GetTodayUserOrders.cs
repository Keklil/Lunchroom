using MediatR;
using Contracts.Repositories;
using Domain.DataTransferObjects.User;

namespace Application.Queries;

public record GetTodayUserOrdersQuery(Guid UserId, Guid GroupId) 
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
        var listTodayOrder = await _repository.Order.GetTodayOrdersByUserAsync(request.UserId, request.GroupId);

        return listTodayOrder;
    }
}