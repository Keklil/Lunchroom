using Contracts.Repositories;
using Domain.Exceptions;
using MediatR;
using Shared.DataTransferObjects.Menu;

namespace Application.Queries;

public sealed record GetMenuQuery(DateTime Date, Guid GroupId) :
    IRequest<MenuDto>;

internal class GetMenuQueryHandler : IRequestHandler<GetMenuQuery, MenuDto>
{
    private readonly IRepositoryManager _repository;

    public async Task<MenuDto> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        var menu = await _repository.Menu.GetMenuByDateAsync(request.Date, request.GroupId);
        if (menu is null)
            throw new NotFoundException($"Меню не найдено для даты: {request.Date.Date}");
        
        return menu.Map();
    }

    public GetMenuQueryHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }
}