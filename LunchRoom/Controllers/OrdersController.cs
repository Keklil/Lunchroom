using Application.Queries;
using Application.Commands;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.Order;
using Domain.DataTransferObjects.User;
using Domain.ErrorModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunchRoom.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPublisher _publisher;

        public OrdersController(ISender sender, IPublisher publisher)
        {
            _sender = sender;
            _publisher = publisher;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderForCreationDto orderDto)
        {
            var order = await _sender.Send(new CreateOrderCommand(orderDto));

            return Ok(order);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId)
        {
            var order = await _sender.Send(new GetOrderQuery(orderId));

            return Ok(order);
        }
        
        /// <summary>
        /// История всех заказов пользователя
        /// </summary>
        /// <returns>Возвращает список идентификаторов и дат заказов пользователя</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrdersForUser>>> GetOrdersByUser(Guid userId, Guid groupId)
        {
            var ordersList = await _sender.Send(new GetOrdersByUser(userId, groupId));
            if (ordersList is null)
                return Ok(new { });
            return Ok(ordersList);
        }

        /// <summary>
        /// Сегоднящние заказы пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns>Возвращает список сегоднящних заказов пользователя.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrdersForUser>>> GetTodayOrdersByUser(Guid userId, Guid groupId)
        {
            var ordersList = await _sender.Send(new GetTodayUserOrdersQuery(userId, groupId));
            if (ordersList is null)
                return Ok(new { });
            return Ok(ordersList);
        }

        /// <summary>
        /// Сводка заказов по дате.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OrderReportDto>>> GetOrdersReportByDay(DateTime date, Guid groupId)
        {
            var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var orderReport = await _sender.Send(new GetOrdersReportQuery(dateSearch, groupId));

            return Ok(orderReport);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _sender.Send(new DeleteOrderCommand(orderId));

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ConfirmPayment(Guid orderId)
        {
            var result = await _sender.Send(new ConfirmPaymentCommand(orderId));

            return Ok();
        }
    }
}
