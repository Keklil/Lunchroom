﻿using Contracts.Repositories;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Menu;

namespace Application.Commands;

public sealed record CreateMenuCommand(MenuForCreationDto Menu, Guid GroupId) : IRequest<MenuDto>;

internal sealed class CreateMenuHandler : IRequestHandler<CreateMenuCommand, MenuDto>
{
    private readonly ILogger<CreateMenuHandler> _logger;
    private readonly IRepositoryManager _repository;

    public async Task<MenuDto> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        var dateToday = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        var todayMenu = await _repository.Menu.GetMenuByDateAsync(dateToday, request.GroupId);
        if (todayMenu is not null)
        {
            _logger.LogWarning("Меню на сегодня уже было загружено");
            return null;
        }

        var menuEntity = new Menu(request.GroupId);
        foreach (var item in request.Menu.LunchSets) menuEntity.AddLunchSet(item.Price, item.LunchSetList);
        foreach (var item in request.Menu.Options) menuEntity.AddOption(item.Name, item.Price);

        _repository.Menu.CreateMenu(menuEntity);
        await _repository.SaveAsync(cancellationToken);

        var menuToReturn = menuEntity.Map();

        return menuToReturn;
    }

    public CreateMenuHandler(IRepositoryManager repository, ILogger<CreateMenuHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}