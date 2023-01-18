using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;

namespace Application.Queries;

public sealed record GetUserGroupIdsQuery(Guid Id) :
    IRequest<List<Guid>>;

internal class GetUserGroupIdsHandler : IRequestHandler<GetUserGroupIdsQuery, List<Guid>>
{
    private readonly IRepositoryManager _repository;

    public GetUserGroupIdsHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<List<Guid>> Handle(GetUserGroupIdsQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _repository.User.GetUserAsync(request.Id, true);
        if (userEntity is null)
            throw new UserNotFoundException(request.Id);

        var userGroups = await _repository.User.GetUserGroupIdsAsync(request.Id);
            
        return userGroups;
    }
}