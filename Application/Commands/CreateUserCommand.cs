using AutoMapper;
using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.User;
using Domain.Models;
using MediatR;

namespace Application.Commands
{
	public sealed record CreateUserCommand(UserForCreationDto User) : IRequest<UserDto>;

	internal sealed class CreateCompanyHandler : IRequestHandler<CreateUserCommand, UserDto>
	{
		private readonly IRepositoryManager _repository;
		private readonly IMapper _mapper;

		public CreateCompanyHandler(IRepositoryManager repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			/*var userEntity = new User(request.User.Name,
				request.User.Surname,
				request.User.Surname,
				request.User.Email);
			
			_repository.User.CreateUser(userEntity);
			await _repository.SaveAsync();

			var userToReturn = _mapper.Map<UserDto>(userEntity);*/

			return null;
		}
	}
}
