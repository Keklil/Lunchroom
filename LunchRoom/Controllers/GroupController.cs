using Application.Commands.Groups;
using Application.Queries.Groups;
using Contracts;
using Domain.DataTransferObjects.Group;
using Domain.ErrorModel;
using LunchRoom.Controllers.Infrastructure;
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
    
    /// <summary>
    /// Создать группу.
    /// </summary>
    /// <param name="group"></param>
    /// <remarks>При попытке создания группы пользователем, не являющимся администратором, вернет 400.</remarks>
    [HttpPost]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GroupErrorResponse),StatusCodes.Status400BadRequest)]
    public async Task<GroupDto> Create([FromBody] GroupForCreationDto group)
    {
        var newGroup = await _sender.Send(new CreateGroupCommand(group.AdminId, group.OrganizationName, group.Address));

        return newGroup;
    }
    
    /// <summary>
    /// Добавить пользователя к группе.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    /// <remarks>Если пользователь или группа не найдены, вернет 404,
    /// если пользователь уже состоит в группе, вернет 400.</remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GroupErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
    public async Task AddUser(Guid userId, Guid groupId)
    {
        await _sender.Send(new AddUserToGroupCommand(userId, groupId));
    }

    /// <summary>
    /// Получить информацию о группе.
    /// </summary>
    /// <param name="groupId"></param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
    public async Task<GroupDto> GetGroup(Guid groupId)
    {
        var group = await _sender.Send(new GetGroupQuery(groupId));

        return group;
    }
}