using MediatR;
using Entities.Models;
using Contracts;
using Entities.Exceptions;
using Entities.DataTransferObjects;
using AutoMapper;

namespace Application.Queries
{
    public sealed record GetMenuQuery(DateTime date) :
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
            var menu = await _repository.Menu.GetMenuByDateAsync(request.date);
            if (menu is null)
                throw new NotFoundException($"Menu not found for this date: {request.date.Date}");
            return _mapper.Map<MenuDto>(menu);
        }
    }
}
