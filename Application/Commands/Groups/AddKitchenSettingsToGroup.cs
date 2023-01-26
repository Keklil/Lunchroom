using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects.Group;
using Domain.Exceptions;
using Domain.Exceptions.GroupExceptions;
using MediatR;

namespace Application.Commands.Groups;

public sealed record AddKitchenSettingsToGroupCommand(GroupConfigDto Config) : IRequest<Unit>;

internal sealed class AddKitchenSettingsToGroupHandler : IRequestHandler<AddKitchenSettingsToGroupCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public AddKitchenSettingsToGroupHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(AddKitchenSettingsToGroupCommand request, CancellationToken cancellationToken)
    {
        var groupEntity = await _repository.Groups
            .GetGroupAsync(request.Config.GroupId, true);

        if (groupEntity is null)
            throw new NotFoundException($"Группа с id {request.Config.GroupId} не найдена.");

        var groupSettings = request.Config.Map();
        
        groupEntity.SetSettings(groupSettings);
        _repository.Groups.UpdateGroup(groupEntity);
        await _repository.SaveAsync();
        
        return Unit.Value;
    }
}