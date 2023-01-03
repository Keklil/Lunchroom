using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Contracts.Security;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.User;
using Domain.Models;
using MediatR;

namespace Application.Commands
{
	public sealed record CreateUserCommand(UserRegisterDto User) : IRequest<UserDto>;

	internal sealed class CreateCompanyHandler : IRequestHandler<CreateUserCommand, UserDto>
	{
		private readonly IRepositoryManager _repository;
		private readonly IAuthService _authService;

		public CreateCompanyHandler(IRepositoryManager repository, IAuthService authService)
		{
			_repository = repository;
			_authService = authService;
		}

		public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _authService.RegisterUser(request.User);

			return user.Map();
		}
	}
}
