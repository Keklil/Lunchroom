using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands;

public record DeleteOrderCommand(Guid orderId) : IRequest;

internal sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ILogger<DeleteOrderHandler> _logger;
    private readonly IRepositoryManager _repository;

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _repository.Order.GetOrderAsync(request.orderId);
            if (order is null)
                throw new NotFoundException("Заказ для удаления не найден");

            _repository.Order.DeleteOrder(order);
            await _repository.SaveAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Ошибка при удалении заказа {request.orderId}");
            throw;
        }
    }

    public DeleteOrderHandler(IRepositoryManager repository, ILogger<DeleteOrderHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}