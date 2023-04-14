using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;
using Shared.DataTransferObjects.User;

namespace Application.Queries;

public sealed record GetUserQuery(Guid Id) :
    IRequest<UserDto>;

internal class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IRepositoryManager _repository;

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userEntity = await _repository.User.GetUserAsync(request.Id);

        var user = userEntity.Map();

        return user;
    }

    public GetUserHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}