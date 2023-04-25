using Contracts.Repositories;
using Contracts.Security;
using MediatR;
using Shared.DataTransferObjects.User;

namespace Application.Queries;

public sealed record GetUserQuery : IRequest<UserDto>;

internal class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _repository.User.GetUserAsync(_currentUserService.GetUserId());

        var user = userEntity.Map();

        return user;
    }

    public GetUserHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}