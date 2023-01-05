using Application.Commands.Groups;
using Contracts;
using Domain.DataTransferObjects.Group;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly ISender _sender;
    private readonly IPublisher _publisher;
    
    public GroupController(ISender sender,
        IPublisher publisher,
        ILoggerManager logger)
    {
        _sender = sender;
        _publisher = publisher;
        _logger = logger;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<GroupDto> Create([FromBody] GroupForCreationDto group)
    {
        var newGroup = await _sender.Send(new CreateGroupCommand(group.AdminId, group.OrganizationName, group.Address));

        return newGroup;
    }
}