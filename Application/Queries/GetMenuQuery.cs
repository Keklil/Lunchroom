using MediatR;
using Domain.Models;
using Contracts;
using Domain.Exceptions;
using AutoMapper;
using Contracts.Repositories;
using Domain.DataTransferObjects.Menu;

namespace Application.Queries
{
    public sealed record GetMenuQuery(DateTime Date, Guid GroupId) :
        IRequest<MenuDto>;
    internal class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, MenuDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetMenuQueryHandler(IRepositoryManager repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MenuDto> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menu = await _repository.Menu.GetMenuByDateAsync(request.Date, request.GroupId);
            if (menu is null)
                throw new NotFoundException($"Меню не найдено для даты: {request.Date.Date}");
            return _mapper.Map<MenuDto>(menu);
        }
    }
}
