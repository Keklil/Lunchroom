using MediatR;
using Contracts.Repositories;
using Domain.DataTransferObjects.User;


namespace Application.Queries
{
    public sealed record GetOrdersByUser(Guid userId)
        : IRequest<List<OrdersForUser>>;

    internal class GetOrdersByUserHandler : IRequestHandler<GetOrdersByUser, List<OrdersForUser>>
    {
        private readonly IRepositoryManager _repository;

        public GetOrdersByUserHandler(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<List<OrdersForUser>> Handle(GetOrdersByUser request, CancellationToken cancellationToken)
        {
            var listOrders = await _repository.Order.GetOrdersByUserAsync(request.userId);

            return listOrders;
        }
    }
}
