using Application.Queries;
using Application.Commands;
using Entities.DataTransferObjects;
using Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.MailService;

namespace LunchRoom.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "admin,user")]
    [ApiController]
    [Produces("application/json")]
    public class MenuController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ISender _sender;
        private readonly IPublisher _publisher;

        public MenuController(ISender sender,
            IPublisher publisher,
            ILoggerManager logger)
        {
            _sender = sender;
            _publisher = publisher;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MenuDto>> GetTodayMenu()
        {
            var dateSearch = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            
            var menu = await _sender.Send(new GetMenuQuery(dateSearch));

            return Ok(menu);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MenuDto>> GetMenuByDate(DateTime date)
        {
            var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var menu = await _sender.Send(new GetMenuQuery(dateSearch));

            return Ok(menu);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MenuForList>>> GetAllMenus()
        {
            var menus = await _sender.Send(new GetAllMenusQuery());

            return Ok(menus);
        }
    }
}
