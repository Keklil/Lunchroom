using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using MediatR;

namespace Application.Commands;

public sealed record UpdateUserCommand(Guid UserId, UserForCreationDto User) : IRequest<UserDto>;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await _repository.User.GetUserAsync(request.UserId, true);
        if (userEntity is null)
            throw new NotFoundException("User not found");

        userEntity.ChangeName(request.User.Surname, request.User.Name, request.User.Patronymic);
        _repository.User.UpdateUser(userEntity);
        await _repository.SaveAsync();

        var user = _mapper.Map<UserDto>(userEntity);
        return user;
    }
}
