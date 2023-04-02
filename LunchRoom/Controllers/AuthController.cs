using Application.Commands;
using Application.Commands.Users;
using Domain.DataTransferObjects.User;
using LunchRoom.Controllers.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunchRoom.Controllers;

[AllowAnonymous]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IPublisher _publisher;
    private readonly ISender _sender;

    /// <summary>
    ///     Регистрация администратора.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> RegisterAdmin([FromBody] UserRegisterDto login)
    {
        var admin = await _sender.Send(new CreateAdminCommand(login));

        return admin;
    }

    /// <summary>
    ///     Регистрация пользователя.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> RegisterUser([FromBody] UserRegisterDto login)
    {
        var admin = await _sender.Send(new CreateUserCommand(login));

        return admin;
    }

    /// <summary>
    ///     Авторизация пользователя.
    /// </summary>
    /// <param name="login"></param>
    /// <remarks>
    ///     Возрвращает токен авторизации, содержит пользовательский id, email, роль.
    ///     Если пользователь не подтвердил email, введены неверные данные авторизации - вернет 400.
    ///     Если пользователь не существует - 404.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> Auth([FromBody] UserLogin login)
    {
        var token = await _sender.Send(new LoginCommand(login));

        return token;
    }

    /// <summary>
    ///     Подтверждение почты пользователя.
    /// </summary>
    /// <param name="token"></param>
    [HttpPost]
    public async Task<ActionResult> ConfirmEmail(string token)
    {
        var email = await _sender.Send(new ConfirmEmailCommand(token));

        return Ok(new { Email = email });
    }

    public AuthController(ISender sender,
        IPublisher publisher)
    {
        _sender = sender;
        _publisher = publisher;
    }
}