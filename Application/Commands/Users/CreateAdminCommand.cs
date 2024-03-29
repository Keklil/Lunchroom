﻿using AutoMapper;
using Contracts.Repositories;
using Contracts.Security;
using Domain.DataTransferObjects.User;
using MediatR;

namespace Application.Commands.Users;

public sealed record CreateAdminCommand(UserRegisterDto Admin) : IRequest<UserDto>;

internal sealed class CreateAdminHandler : IRequestHandler<CreateAdminCommand, UserDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IAuthService _authService;

    public CreateAdminHandler(IRepositoryManager repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<UserDto> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterAdmin(request.Admin);

        return user.Map();
    }
}