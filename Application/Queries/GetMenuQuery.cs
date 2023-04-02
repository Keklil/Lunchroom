using AutoMapper;
using Contracts.Repositories;
using Domain.DataTransferObjects.Menu;
using Domain.Exceptions;
using MediatR;

namespace Application.Queries;

public sealed record GetMenuQuery(DateTime Date, Guid GroupId) :
    IRequest<MenuDto>;

internal class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, MenuDto>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public async Task<MenuDto> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        var menu = await _repository.Menu.GetMenuByDateAsync(request.Date, request.GroupId);
        if (menu is null)
            throw new NotFoundException($"Меню не найдено для даты: {request.Date.Date}");
        return _mapper.Map<MenuDto>(menu);
    }

    public GetMenuQueryHandler(IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}