using Contracts.Repositories;
using MediatR;
using Shared.DataTransferObjects.Order;

namespace Application.Queries;

public sealed record GetOrderQuery(Guid OrderId) : IRequest<OrderDto>;

internal class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderDto>
{
    private readonly IRepositoryManager _repository;

    public async Task<OrderDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.Order.GetOrderAsync(request.OrderId, false);

        var order = orderEntity.Map();

        return order;
    }

    public GetOrderHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}