using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects;
using Domain.Exceptions;
using MediatR;
using Repository;

namespace Application.Commands;

public sealed record ConfirmPaymentCommand(Guid orderId) : IRequest;

internal sealed class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    
    public ConfirmPaymentHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
    
    public async Task<Unit> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.Order.GetOrderAsync(request.orderId, true);
        if (orderEntity is null)
            throw new NotFoundException("Подтверждаемы заказ не найден");
        
        orderEntity.ConfirmPayment();
        
        _repository.Order.UpdateOrder(orderEntity);

        await _repository.SaveAsync();
        
        return Unit.Value;
    }
}