using System.ComponentModel;
using Application.Queries.Enums;
using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using MediatR;
using Shared.DataTransferObjects.Menu;

namespace Application.Queries;

public sealed record GetMenuQuery(DateTime Date, QueryFor QueryFor, Guid TargetId) :
    IRequest<MenuDto>;

internal class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, MenuDto>
{
    private readonly IRepositoryManager _repository;

    public async Task<MenuDto> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        Menu menu;

        switch (request.QueryFor)
        {
            case QueryFor.Group:
                var group = await _repository.Groups.GetGroupAsync(request.TargetId);
                if (group.SelectedKitchenId is null)
                    throw new DomainException("Для группы {GroupId} не выбрана столовая", group.Id);
                menu = await _repository.Menu.GetMenuByDateAsync(request.Date, group.SelectedKitchenId.Value);
                break;
            
            case QueryFor.Kitchen:
                menu = await _repository.Menu.GetMenuByDateAsync(request.Date, request.TargetId);
                break;
            
            default:
                throw new InvalidEnumArgumentException();
        }

        return menu.Map();
    }

    public GetMenuQueryHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}