using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using MediatR;
using Shared.DataTransferObjects.User;

namespace Application.Queries;

public sealed record GetUserGroupsQuery :
    IRequest<List<UserGroupDto>>;

internal class GetUserGroupIdsHandler : IRequestHandler<GetUserGroupsQuery, List<UserGroupDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<List<UserGroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
    {
        var userGroups = await _repository.User.GetUserGroupAsync(_currentUserService.GetUserId());

        var userGroupsDto = userGroups.Select(x => new UserGroupDto(x.Id, x.OrganizationName)).ToList();

        return userGroupsDto;
    }

    public GetUserGroupIdsHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}