using AutoMapper;
using MediatR;
using Domain.Models;
using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects.Menu;
using Domain.Exceptions;

namespace Application.Queries;

public sealed record GetAllMenusQuery() : IRequest<List<MenuForList>>;

internal class GetAllMenusHandler : IRequestHandler<GetAllMenusQuery, List<MenuForList>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    
    public GetAllMenusHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<MenuForList>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        var listRawMenus = await _repository.Menu.GetMenus();
        List<MenuForList> listMenus = new();
        if (listRawMenus is null)
            return listMenus;

        foreach (var rawMenu in listRawMenus)
        {
            var menu = _mapper.Map<MenuForList>(rawMenu);
            listMenus.Add(menu);
        }

        return listMenus;
    }
}