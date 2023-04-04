using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands;

public sealed record ConfirmPaymentCommand(Guid orderId) : IRequest;

internal sealed class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand>
{
    private readonly IRepositoryManager _repository;

    public async Task Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.Order.GetOrderAsync(request.orderId);
        if (orderEntity is null)
            throw new NotFoundException("Подтверждаемы заказ не найден");

        orderEntity.ConfirmPayment();

        _repository.Order.UpdateOrder(orderEntity);

        await _repository.SaveAsync();
    }

    public ConfirmPaymentHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}