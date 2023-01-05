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
   public AuthCodes Code { get; set; }
    
}

public enum AuthCodes
{
    UserExists = 1001,
    WrongCredentials = 1002
}