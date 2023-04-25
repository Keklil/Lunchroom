using Contracts.Repositories;
using Contracts.Security;
using Domain.Exceptions;
using MediatR;
using Shared.DataTransferObjects.User;

namespace Application.Commands;

public sealed record UpdateUserCommand(UserForCreationDto User) : IRequest<UserDto>;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ICurrentUserService _currentUserService;

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await _repository.User.GetUserAsync(_currentUserService.GetUserId());
        if (userEntity is null)
            throw new NotFoundException("User not found");

        userEntity.ChangeName(request.User.Surname, request.User.Name, request.User.Patronymic);
        userEntity.ChangePhone(request.User.Phone);
        _repository.User.UpdateUser(userEntity);
        await _repository.SaveAsync(cancellationToken);

        var user = userEntity.Map();
        return user;
    }

    public UpdateUserCommandHandler(IRepositoryManager repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }
}