using Contracts.Repositories;
using Contracts.Security;
using MediatR;
using Shared.DataTransferObjects.User;

namespace Application.Queries;

public sealed record GetUserQuery(Guid? UserId = null) : IRequest<UserDto>;

internal class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _repository.User.GetUserAsync(request.UserId ?? _currentUserService.GetUserId());
        var kitchenIds = await _repository.User.GetUserKitchenIdsAsync(userEntity.Id);

        var user = userEntity.Map(kitchenIds);

        return user;
    }

    public GetUserHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}