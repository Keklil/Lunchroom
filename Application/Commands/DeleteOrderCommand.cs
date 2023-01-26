using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects;
using Domain.Exceptions;
using Domain.Models;
using MediatR;

namespace Application.Commands;

public record DeleteOrderCommand(Guid orderId) : IRequest;

internal sealed class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteOrderHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _repository.Order.GetOrderAsync(request.orderId, true);
            if (order is null)
                throw new NotFoundException("Заказ для удаления не найден");
            
            _repository.Order.DeleteOrder(order);        
            await _repository.SaveAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Ошибка при удалении: {e}");
            throw;
        }

        return Unit.Value;
    }
}