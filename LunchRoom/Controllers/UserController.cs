using Application.Commands.Users;
using Application.Queries;
using Domain.ErrorModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.User;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Authorize]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    ///     Получить информацию о пользователе.
    /// </summary>
    /// <param name="userId">При указании параметра выполняется поиск по заданному id. Иначе возвращается информация об авторизованном пользователе.</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(Guid? userId)
    {
        var user = await _sender.Send(new GetUserQuery(userId));

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserForCreationDto updatedUser)
    {
        var user = await _sender.Send(new UpdateUserCommand(updatedUser));

        return Ok(user);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<List<UserGroupDto>> GetUserGroup()
    {
        var groups = await _sender.Send(new GetUserGroupsQuery());

        return groups;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SubscribeToNotifications(string deviceToken)
    {
        await _sender.Send(new SubscribeToNotificationsCommand(deviceToken));
        
        return Ok();
    }

    public UserController(ISender sender)
    {
        _sender = sender;
    }
}