using Application.Commands.Groups;
using Application.Queries.Groups;
using Domain.ErrorModel;
using LunchRoom.Controllers.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects.Group;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
[Authorize]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    ///     Создать группу.
    /// </summary>
    /// <param name="group"></param>
    /// <remarks></remarks>
    [HttpPost]
    [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GroupErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<GroupDto> Create([FromBody] GroupForCreationDto group)
    {
        var newGroup = await _sender.Send(new CreateGroupCommand(group.OrganizationName));

        return newGroup;
    }

    /// <summary>
    ///     Добавить пользователя к группе.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    /// <response code="404">Если пользователь или группа не найдены</response>
    /// <response code="400">Если пользователь уже состоит в группе</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GroupErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task AddUser(Guid userId, Guid groupId)
    {
        await _sender.Send(new AddUserToGroupCommand(userId, groupId));
    }

    /// <summary>
    ///     Получить информацию о группе.
    /// </summary>
    /// <param name="groupId"></param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<GroupDto> GetGroup(Guid groupId)
    {
        var group = await _sender.Send(new GetGroupQuery(groupId));

        return group;
    }

    /// <summary>
    ///     Установить данные локации для группы.
    /// </summary>
    /// <param name="config"></param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task ConfigureGroupLocation([FromBody] GroupConfigByAddressDto config)
    {
        await _sender.Send(new AddLocationInfoToGroupCommand(config));
    }
    
    /// <summary>
    ///     Привязать столовую к группе, в которой будет происходить заказ
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="kitchenId"></param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task SetActiveKitchen(Guid groupId, Guid kitchenId)
    {
        await _sender.Send(new SetActiveKitchenCommand(groupId, kitchenId));
    }

    /// <summary>
    ///     Установить информацию для платежей, которая будет отображаться у заказчиков.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="paymentInfoDto"></param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task ConfigurePaymentInfo(Guid groupId, [FromBody] PaymentInfoDto paymentInfoDto)
    {
        await _sender.Send(new AddPaymentInfoToGroupCommand(groupId, paymentInfoDto));
    }

    /// <summary>
    ///     Получить список столовых, в зоне доставки которых находится группа.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task<List<AvailableKitchensDto>> GetAvailableKitchens(Guid groupId)
    {
        var groups = await _sender.Send(new GetAvailableKitchensQuery(groupId));

        return groups;
    }

    public GroupController(ISender sender)
    {
        _sender = sender;
    }
}