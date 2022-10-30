using AutoMapper;
using MediatR;
using Entities.Models;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Exceptions;

namespace Application.Queries
{
    public sealed record GetUserQuery(Guid Id) :
        IRequest<UserDto>;

    internal class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetUserHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var userEntity = await _repository.User.GetUserAsync(request.Id, true);
            if (userEntity is null)
                throw new UserNotFoundException(request.Id);

            var user = _mapper.Map<UserDto>(userEntity);
            if (user.Name.Length > 0 && user.Surname.Length > 0)
                user.NameFill = true;
            
            return user;
        }
    }
}
