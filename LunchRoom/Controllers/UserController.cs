﻿using Application.Queries;
using Application.Commands;
using Entities.DataTransferObjects;
using Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Authorize(Roles = "admin,user")]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly ISender _sender;

    public UserController(ISender sender, ILoggerManager logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(Guid userId)
    {
        var user = await _sender.Send(new GetUserQuery(userId));
        
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid userId, [FromBody] UserForCreationDto updatedUser)
    {
        var user = await _sender.Send(new UpdateUserCommand(userId, updatedUser));

        return Ok(user);
    }
}