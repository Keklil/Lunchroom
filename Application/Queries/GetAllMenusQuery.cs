using AutoMapper;
using Contracts.Repositories;
using Domain.DataTransferObjects.Menu;
using MediatR;

namespace Application.Queries;

public sealed record GetAllMenusQuery(Guid GroupId) : IRequest<List<MenuForList>>;

internal class GetAllMenusHandler : IRequestHandler<GetAllMenusQuery, List<MenuForList>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public async Task<List<MenuForList>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        var listRawMenus = await _repository.Menu.GetMenus(request.GroupId);
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

    public GetAllMenusHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}