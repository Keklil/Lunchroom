using Contracts.Repositories;
using MediatR;
using Shared.DataTransferObjects.Menu;

namespace Application.Queries;

public sealed record GetAllMenusQuery(Guid GroupId) : IRequest<List<MenuForList>>;

internal class GetAllMenusHandler : IRequestHandler<GetAllMenusQuery, List<MenuForList>>
{
    private readonly IRepositoryManager _repository;

    public async Task<List<MenuForList>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        var menuEntities = await _repository.Menu.GetMenuByGroup(request.GroupId);
        if (menuEntities.Count == 0)
            return new List<MenuForList>();
        
        var listMenus = new List<MenuForList>();
        foreach (var menuEntity in menuEntities)
        {
            var menu = menuEntity.MapToMenuForList();
            listMenus.Add(menu);
        }

        return listMenus;
    }

    public GetAllMenusHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}