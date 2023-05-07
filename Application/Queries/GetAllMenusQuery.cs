using System.ComponentModel;
using Application.Queries.Enums;
using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Shared.DataTransferObjects.Menu;

namespace Application.Queries;

public sealed record GetAllMenusQuery(QueryFor QueryFor, Guid TargetId) : IRequest<List<MenuForHistoryDto>>;

internal class GetAllMenusHandler : IRequestHandler<GetAllMenusQuery, List<MenuForHistoryDto>>
{
    private readonly IRepositoryManager _repository;

    public async Task<List<MenuForHistoryDto>> Handle(GetAllMenusQuery request, CancellationToken cancellationToken)
    {
        List<Menu> menuEntities;

        switch (request.QueryFor)
        {
            case QueryFor.Group:
                var group = await _repository.Groups.GetGroupAsync(request.TargetId);
                if (group.SelectedKitchenId is null)
                    throw new DomainException("Для группы {GroupId} не выбрана столовая", group.Id);
                menuEntities = await _repository.Menu.GetMenuByGroup(group.SelectedKitchenId.Value);
                break;
            
            case QueryFor.Kitchen:
                menuEntities = await _repository.Menu.GetMenuByGroup(request.TargetId);
                break;
            
            default:
                throw new InvalidEnumArgumentException();
        }
        
        if (menuEntities.Count == 0)
            return new List<MenuForHistoryDto>();
        
        var listMenus = new List<MenuForHistoryDto>();
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