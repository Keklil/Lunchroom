using Application.Commands.Menu;
using Application.Notifications;
using Application.Queries;
using Application.Queries.Enums;
using Domain.ErrorModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.Menu;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class MenuController : ControllerBase
{
    private readonly IPublisher _publisher;
    private readonly ISender _sender;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetTodayMenuForGroup(Guid groupId)
    {
        var dateSearch = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

        var menu = await _sender.Send(new GetMenuQuery(dateSearch, QueryFor.Group, groupId));

        return Ok(menu);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetMenuByDateForGroup(DateTime date, Guid groupId)
    {
        var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        var menu = await _sender.Send(new GetMenuQuery(dateSearch, QueryFor.Group, groupId));

        return Ok(menu);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuDto>> GetMenuByDateForKitchen(DateTime date, Guid kitchenId)
    {
        var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        var menu = await _sender.Send(new GetMenuQuery(dateSearch, QueryFor.Kitchen, kitchenId));

        return Ok(menu);
    }

    /// <summary>
    ///     История меню для группы
    /// </summary>
    /// <returns>Возвращает список идентификаторов и дат загрузки меню</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<MenuForHistoryDto>>> GetAllMenusForGroup(Guid groupId)
    {
        var menus = await _sender.Send(new GetAllMenusQuery(QueryFor.Group, groupId));

        return Ok(menus);
    }
    
    /// <summary>
    ///     История всех загруженных столовой меню
    /// </summary>
    /// <returns>Возвращает список идентификаторов и дат загрузки меню</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MenuForHistoryDto>>> GetAllMenusForKitchen(Guid kitchenId)
    {
        var menus = await _sender.Send(new GetAllMenusQuery(QueryFor.Kitchen, kitchenId));

        return Ok(menus);
    }

    /// <summary>
    ///     Загрузка меню
    /// </summary>
    /// <param name="kitchenId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadMenu(Guid kitchenId, [FromBody] RawMenuDto request)
    {
        var rawMenu = string.Join("\n", request.Menu);
        await _publisher.Publish(new UploadMenu(rawMenu, request.GroupId));

        return Ok();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ActivateMenu(Guid menuId)
    {
        await _sender.Send(new PublishMenuCommand(menuId));
        
        return Ok();
    }

    /// <summary>
    ///     Загрузка меню из файла
    /// </summary>
    /// <param name="kitchenId"></param>
    /// <param name="menu"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadMenuFromFile(Guid kitchenId, IFormFile menu)
    {
        await _publisher.Publish(new UploadMenuFromFile(kitchenId, menu));

        return Ok();
    }

    public MenuController(ISender sender,
        IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }
}