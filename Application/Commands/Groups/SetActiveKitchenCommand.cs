using Application.Interfaces;
using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using Domain.Exceptions.AuthExceptions;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Group;

namespace Application.Commands.Groups;

public sealed record SetActiveKitchenCommand(Guid GroupId, Guid KitchenId) : IRequest<GroupDto>, IGroupRequest;

internal sealed class SetActiveKitchenHandler : IRequestHandler<SetActiveKitchenCommand, GroupDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<GroupDto> Handle(SetActiveKitchenCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.GetUserId();
        var admin = await _repository.User.GetUserAsync(currentUserId);

        var group = await _repository.Groups.GetGroupAsync(request.GroupId);
        
        var kitchen = await _repository.Kitchens.GetKitchenAsync(request.KitchenId);
        
        group.SelectKitchen(kitchen);

        _repository.Groups.UpdateGroup(group);
        await _repository.SaveAsync(cancellationToken);

        return group.Map();
    }

    public SetActiveKitchenHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}