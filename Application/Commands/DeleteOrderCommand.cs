using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
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
            _repository.Order.DeleteOrder(order);        
            await _repository.SaveAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Delete exception: {e}");
            throw;
        }

        return Unit.Value;
    }
}