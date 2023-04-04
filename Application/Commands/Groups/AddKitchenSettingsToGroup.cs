using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Group;

namespace Application.Commands.Groups;

public sealed record AddKitchenSettingsToGroupCommand(GroupConfigDto Config) : IRequest<Unit>;

internal sealed class AddKitchenSettingsToGroupHandler : IRequestHandler<AddKitchenSettingsToGroupCommand, Unit>
{
    private readonly IRepositoryManager _repository;

    public async Task<Unit> Handle(AddKitchenSettingsToGroupCommand request, CancellationToken cancellationToken)
    {
        var groupEntity = await _repository.Groups
            .GetGroupAsync(request.Config.GroupId);

        if (groupEntity is null)
            throw new NotFoundException($"Группа с id {request.Config.GroupId} не найдена.");

        var groupSettings = request.Config.Map();

        groupEntity.SetSettings(groupSettings);
        _repository.Groups.UpdateGroup(groupEntity);
        await _repository.SaveAsync();

        return Unit.Value;
    }

    public AddKitchenSettingsToGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}