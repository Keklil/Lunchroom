using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using MediatR;

namespace Application.Queries;

public sealed record GetUserGroupIdsQuery :
    IRequest<List<Guid>>;

internal class GetUserGroupIdsHandler : IRequestHandler<GetUserGroupIdsQuery, List<Guid>>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<List<Guid>> Handle(GetUserGroupIdsQuery request, CancellationToken cancellationToken)
    {
        var userGroups = await _repository.User.GetUserGroupIdsAsync(_currentUserService.GetUserId());

        return userGroups;
    }

    public GetUserGroupIdsHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}