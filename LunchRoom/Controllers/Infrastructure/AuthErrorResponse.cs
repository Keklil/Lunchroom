namespace LunchRoom.Controllers.Infrastructure;

public class AuthErrorResponse
{
    public enum ErrorCodes
    {
        UserExists = 1001,
        UnconfirmedEmail = 1002,
        WrongCredentials = 1003
    }

    /// <summary>
    ///     Описание возникшей ошибки
    /// </summary>
    public string? ExceptionMessage { get; set; }

    /// <summary>
    ///     Код ошибки
    /// </summary>
    public ErrorCodes Code { get; set; }
}