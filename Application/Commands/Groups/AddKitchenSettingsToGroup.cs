using Contracts.Repositories;
using MediatR;
using Shared.DataTransferObjects.Group;

namespace Application.Commands.Groups;

public sealed record AddKitchenSettingsToGroupCommand(GroupConfigDto Config) : IRequest;

internal sealed class AddKitchenSettingsToGroupHandler : IRequestHandler<AddKitchenSettingsToGroupCommand>
{
    private readonly IRepositoryManager _repository;

    public async Task Handle(AddKitchenSettingsToGroupCommand request, CancellationToken cancellationToken)
    {
        var groupEntity = await _repository.Groups.GetGroupAsync(request.Config.GroupId);

        var groupSettings = request.Config.Map();

        groupEntity.SetSettings(groupSettings);
        _repository.Groups.UpdateGroup(groupEntity);
        await _repository.SaveAsync(cancellationToken);
    }

    public AddKitchenSettingsToGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}