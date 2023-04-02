using Contracts.Repositories;
using Domain.DataTransferObjects.User;
using MediatR;

namespace Application.Queries;

public sealed record GetOrdersByUser(Guid UserId, Guid GroupId)
    : IRequest<List<OrdersForUser>>;

internal class GetOrdersByUserHandler : IRequestHandler<GetOrdersByUser, List<OrdersForUser>>
{
    private readonly IRepositoryManager _repository;

    public async Task<List<OrdersForUser>> Handle(GetOrdersByUser request, CancellationToken cancellationToken)
    {
        var listOrders = await _repository.Order.GetOrdersByUserAsync(request.UserId, request.GroupId);

        return listOrders;
    }

    public GetOrdersByUserHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}