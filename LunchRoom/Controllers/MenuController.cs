﻿using Application.Notifications;
using Application.Queries;
using Domain.ErrorModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.Menu;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Authorize(Roles = "Admin,User")]
[ApiController]
[Produces("application/json")]
public class MenuController : ControllerBase
{
    private readonly IPublisher _publisher;
    private readonly ISender _sender;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetTodayMenu(Guid groupId)
    {
        var dateSearch = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var menu = await _sender.Send(new GetMenuQuery(dateSearch, groupId));

        return Ok(menu);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetMenuByDate(DateTime date, Guid groupId)
    {
        var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        var menu = await _sender.Send(new GetMenuQuery(dateSearch, groupId));

        return Ok(menu);
    }

    /// <summary>
    ///     История всех загруженных меню
    /// </summary>
    /// <returns>Возвращает список идентификаторов и дат загрузки меню</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuForList>>> GetAllMenus(Guid groupId)
    {
        var menus = await _sender.Send(new GetAllMenusQuery(groupId));

        return Ok(menus);
    }

    /// <summary>
    ///     Загрузка меню
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadMenu([FromBody] RawMenuDto request)
    {
        var rawMenu = string.Join("\n", request.Menu);
        await _publisher.Publish(new EmailWithMenuFetched(rawMenu, request.GroupId));

        return Ok();
    }

    public MenuController(ISender sender,
        IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }
}