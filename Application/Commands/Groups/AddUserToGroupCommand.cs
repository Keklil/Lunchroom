using Contracts.Repositories;
using Domain.Exceptions.GroupExceptions;
using MediatR;

namespace Application.Commands.Groups;

public sealed record AddUserToGroupCommand(Guid UserId, Guid GroupId) : IRequest;

internal sealed class AddUserToGroupHandler : IRequestHandler<AddUserToGroupCommand>
{
    private readonly IRepositoryManager _repository;

    public async Task Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _repository.Groups.GetGroupAsync(request.GroupId);

        var user = await _repository.User.GetUserAsync(request.UserId);

        if (group.Members.Any(x => x.Id == user.Id))
            throw new UserAlreadyInGroupException();

        group.AddMember(user);

        _repository.Groups.UpdateGroup(group);
        await _repository.SaveAsync(cancellationToken);
    }

    public AddUserToGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}