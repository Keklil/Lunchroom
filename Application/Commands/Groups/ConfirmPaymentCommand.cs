using Contracts.Repositories;
using MediatR;

namespace Application.Commands.Groups;

public sealed record ConfirmPaymentCommand(Guid OrderId) : IRequest;

internal sealed class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand>
{
    private readonly IRepositoryManager _repository;

    public async Task Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.Order.GetOrderAsync(request.OrderId);

        orderEntity.ConfirmPayment();

        _repository.Order.UpdateOrder(orderEntity);

        await _repository.SaveAsync(cancellationToken);
    }

    public ConfirmPaymentHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}