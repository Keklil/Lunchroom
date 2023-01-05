using Application.Queries;
using Application.Commands;
using Application.Commands.Users;
using Contracts;
using Domain.DataTransferObjects.User;
using LunchRoom.Controllers.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.MailService;

namespace LunchRoom.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ISender _sender;
        private readonly IPublisher _publisher;

        public AuthController(ISender sender,
            IPublisher publisher,
            ILoggerManager logger)
        {
            _sender = sender;
            _publisher = publisher;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> RegisterAdmin([FromBody] UserRegisterDto login)
        {
            var admin = await _sender.Send(new CreateAdminCommand(login));

            return admin;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserRegisterDto login)
        {
            var admin = await _sender.Send(new CreateUserCommand(login));

            return admin;
        }
        
        /// <summary>
        /// Авторизация пользователя.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Возрвращает токен авторизации, содержит пользовательский id, email, роль.
        /// Возвращает null, если пользователь не существует, неверные данные авторизации.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> Auth([FromBody] UserLogin login)
        {
            var token = await _sender.Send(new LoginCommand(login));

            return token;
        }
        
        [HttpPost]
        public async Task<ActionResult> ConfirmEmail(string token)
        {
            var email = await _sender.Send(new ConfirmEmailCommand(token));
            
            return Ok(new { Email = email });
        }
    }
}
