using Application.Commands.Kitchens;
using Application.Queries.Kitchen;
using Domain.ErrorModel;
using LunchRoom.Controllers.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using Shared.DataTransferObjects.Kitchen;

namespace LunchRoom.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
[Authorize(Roles = "KitchenOperator")]
[ApiController]
public class KitchenController : ControllerBase
{
    private readonly IPublisher _publisher;
    private readonly ISender _sender;

    /// <summary>
    ///     Создать столовую.
    /// </summary>
    /// <param name="kitchen"></param>
    /// <remarks></remarks>
    [HttpPost]
    [ProducesResponseType(typeof(KitchenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<KitchenDto> Create([FromBody] KitchenForCreationDto kitchen)
    {
        var newGroup = await _sender.Send(new CreateKitchenCommand(kitchen));

        return newGroup;
    }

    /// <summary>
    ///     Добавить менеджера к столовой.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="kitchenId"></param>
    /// <response code="404">Если пользователь или кухня не найдены</response>
    /// <response code="400">Если пользователь уже является менеджером столовой</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GroupErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task AddUser(Guid userId, Guid kitchenId)
    {
        await _sender.Send(new AddUserToKitchenCommand(userId, kitchenId));
    }

    /// <summary>
    ///     Получить информацию о столовой.
    /// </summary>
    /// <param name="kitchenId"></param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    public async Task<KitchenDto> GetKitchen(Guid kitchenId)
    {
        var kitchen = await _sender.Send(new GetKitchenQuery(kitchenId));

        return kitchen;
    }

    /// <summary>
    ///     Изменить настройки столовой.
    /// </summary>
    /// <param name="kitchenId"></param>
    /// <param name="settings"></param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task ConfigureKitchen(Guid kitchenId, [FromBody] KitchenSettingsForEditDto settings)
    {
        await _sender.Send(new EditKitchenSettingsCommand(kitchenId, settings));
    }

    /// <summary>
    ///  Добавить или изменить область доставки.
    /// </summary>
    /// <param name="kitchenId"></param>
    /// <param name="areas"></param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task ConfigureShippingAreas(Guid kitchenId, [FromBody] List<Polygon> areas)
    {
        await _sender.Send(new EditKitchenShippingAreasCommand(kitchenId, areas));
    }
    
    /// <summary>
    ///  Подтвердить, что столовая проверена модератером.
    /// </summary>
    /// <param name="kitchenId"></param>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
    public async Task KitchenIsVerify(Guid kitchenId)
    {
        await _sender.Send(new VerifyKitchenCommand(kitchenId));
    }
    

    public KitchenController(ISender sender,
        IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }
}