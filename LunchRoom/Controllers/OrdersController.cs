using Application.Queries;
using Application.Commands;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.Order;
using Domain.DataTransferObjects.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunchRoom.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin,user")]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId)
        {
            var order = await _sender.Send(new GetOrderQuery(orderId));

            return Ok(order);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrdersForUser>>> GetOrdersByUser(Guid userId)
        {
            var ordersList = await _sender.Send(new GetOrdersByUser(userId));
            if (ordersList is null)
                return Ok(new { });
            return Ok(ordersList);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrdersForUser>>> GetTodayOrdersByUser(Guid userId)
        {
            var ordersList = await _sender.Send(new GetTodayUserOrdersQuery(userId));
            if (ordersList is null)
                return Ok(new { });
            return Ok(ordersList);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OrderReportDto>>> GetOrdersReportByDay(DateTime date)
        {
            var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var orderReport = await _sender.Send(new GetOrdersReportQuery(dateSearch));

            return Ok(orderReport);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _sender.Send(new DeleteOrderCommand(orderId));

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ConfirmPayment(Guid orderId)
        {
            var result = await _sender.Send(new ConfirmPaymentCommand(orderId));

            return Ok();
        }
    }
}
