using System.Text.Json.Serialization;

namespace LunchRoom.Controllers.Infrastructure;

public class AuthErrorResponse
{
    /// <summary>
    /// Описание возникшей ошибки
    /// </summary>
    public string? ExceptionMessage { get; set; }
    
    /// <summary>
    /// Код ошибки
    /// </summary>
    public ErrorCodes Code { get; set; }
    
    public enum ErrorCodes
    {
        UserExists = 1001,
    }
}

