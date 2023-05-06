using Contracts.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands;

public record DeleteOrderCommand(Guid OrderId) : IRequest;

internal sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ILogger<DeleteOrderHandler> _logger;
    private readonly IRepositoryManager _repository;

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _repository.Order.GetOrderAsync(request.OrderId);
            
            _repository.Order.DeleteOrder(order);
            
            await _repository.SaveAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Ошибка при удалении заказа {request.OrderId}");
            throw;
        }
    }

    public DeleteOrderHandler(IRepositoryManager repository, ILogger<DeleteOrderHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}