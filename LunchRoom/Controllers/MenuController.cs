using Application.Queries;
using Application.Commands;
using Contracts;
using Domain.DataTransferObjects.Menu;
using Domain.ErrorModel;
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
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MenuDto>> GetTodayMenu(Guid groupId)
        {
            var dateSearch = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            
            var menu = await _sender.Send(new GetMenuQuery(dateSearch, groupId));

            return Ok(menu);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MenuDto>> GetMenuByDate(DateTime date, Guid groupId)
        {
            var dateSearch = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            var menu = await _sender.Send(new GetMenuQuery(dateSearch, groupId));

            return Ok(menu);
        }
        
        /// <summary>
        /// История всех загруженных меню
        /// </summary>
        /// <returns>Возвращает список идентификаторов и дат загрузки меню</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MenuForList>>> GetAllMenus(Guid groupId)
        {
            var menus = await _sender.Send(new GetAllMenusQuery(groupId));

            return Ok(menus);
        }
    }
}
