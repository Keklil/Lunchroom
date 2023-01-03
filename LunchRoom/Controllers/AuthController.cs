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
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(AuthErrorResponse), 400)]
        public async Task<ActionResult<UserDto>> RegisterAdmin([FromBody] UserRegisterDto login)
        {
            var admin = await _sender.Send(new CreateAdminCommand(login));

            return admin;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(AuthErrorResponse), 400)]
        public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserRegisterDto login)
        {
            var admin = await _sender.Send(new CreateUserCommand(login));

            return admin;
        }
        
        [HttpPost]
        public async Task<ActionResult<AuthResult>> Auth([FromBody] UserLogin login)
        {
            var token = await _sender.Send(new LoginCommand(login));
            var result = new AuthResult() { Token = token, Message = "Success"};

            if (token is null)
                result.Message = "Check mailbox";
            
            return result;
        }
        
        [HttpPost]
        public async Task<ActionResult> ConfirmEmail(string token)
        {
            var email = await _sender.Send(new ConfirmEmailCommand(token));
            
            return Ok(new { Email = email });
        }
    }
}
