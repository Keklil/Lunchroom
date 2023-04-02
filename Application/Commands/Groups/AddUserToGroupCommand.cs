﻿using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Exceptions.GroupExceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Groups;

public sealed record AddUserToGroupCommand(Guid UserId, Guid GroupId) : IRequest<Unit>;

internal sealed class AddUserToGroupHandler : IRequestHandler<AddUserToGroupCommand, Unit>
{
    private readonly IRepositoryManager _repository;

    public async Task<Unit> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _repository.Groups.GetGroupAsync(request.GroupId, true);
        if (group is null)
            throw new NotFoundException("Запрашиваемая группа не найдена");

        var user = await _repository.User.GetUserAsync(request.UserId, true);
        if (user is null)
            throw new NotFoundException("Не удалось найти пользователя по заданному id");

        if (group.Members.Any(x => x.Id == user.Id))
            throw new UserAlreadyInGroupException();

        group.AddMember(user);

        _repository.Groups.UpdateGroup(group);
        await _repository.SaveAsync();

        return Unit.Value;
    }

    public AddUserToGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}