﻿using Contracts.Repositories;
using Contracts.Security;
using Domain.DataTransferObjects.User;
using MediatR;

namespace Application.Commands;

public sealed record CreateUserCommand(UserRegisterDto User) : IRequest<UserDto>;

internal sealed class CreateCompanyHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IAuthService _authService;
    private readonly IRepositoryManager _repository;

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterUser(request.User);

        return user.Map();
    }

    public CreateCompanyHandler(IRepositoryManager repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }
}