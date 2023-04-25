using Application.Commands;
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var user = await _sender.Send(new GetUserQuery());

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserForCreationDto updatedUser)
    {
        var user = await _sender.Send(new UpdateUserCommand(updatedUser));

        return Ok(user);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<List<Guid>> GetUserGroupIds()
    {
        var groups = await _sender.Send(new GetUserGroupIdsQuery());

        return groups;
    }

    public UserController(ISender sender)
    {
        _sender = sender;
    }
}