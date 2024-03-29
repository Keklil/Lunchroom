﻿using Contracts.Repositories;
using Domain.DataTransferObjects.Group;
using Domain.Exceptions;
using MediatR;

namespace Application.Queries.Groups;

public sealed record GetGroupQuery(Guid Id) : IRequest<GroupDto>;

internal class GetGroupHandler : IRequestHandler<GetGroupQuery, GroupDto>
{
    private readonly IRepositoryManager _repository;

    public GetGroupHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
    
    public async Task<GroupDto> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        var group = await _repository.Groups.GetGroupAsync(request.Id, false);
        if (group is null)
            throw new NotFoundException("Группа не найдена");

        return group.Map();
    }
}