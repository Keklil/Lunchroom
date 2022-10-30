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
        public async Task<ActionResult<AuthResult>> Auth([FromBody] UserLogin login)
        {
            var token = await _sender.Send(new LoginCommand(login));
            var result = new AuthResult() { Token = token, Message = "Success"};

            if (token is null)
                result.Message = "Check mailbox";
            
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<ActionResult> ConfirmEmail(string token)
        {
            var email = await _sender.Send(new ConfirmEmailCommand(token));
            
            return Ok(new { Email = email });
        }
    }
}
