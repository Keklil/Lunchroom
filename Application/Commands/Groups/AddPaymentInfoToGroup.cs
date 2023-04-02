using Contracts.Repositories;
using Domain.DataTransferObjects.Group;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Groups;

public sealed record AddPaymentInfoToGroupCommand(PaymentInfoDto PaymentInfo) : IRequest<Unit>;

internal sealed class AddPaymentInfoToGroupHandler : IRequestHandler<AddPaymentInfoToGroupCommand, Unit>
{
    private readonly IRepositoryManager _repository;

    public async Task<Unit> Handle(AddPaymentInfoToGroupCommand request, CancellationToken cancellationToken)
    {
        var groupEntity = await _repository.Groups
            .GetGroupAsync(request.PaymentInfo.GroupId, true);

        if (groupEntity is null)
            throw new NotFoundException($"Группа с id {request.PaymentInfo.GroupId} не найдена.");

        var payment = request.PaymentInfo.Map();

        groupEntity.SetPaymentInfo(payment);
        _repository.Groups.UpdateGroup(groupEntity);
        await _repository.SaveAsync();

        return Unit.Value;
    }

    public AddPaymentInfoToGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}