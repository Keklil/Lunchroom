namespace LunchRoom.Controllers.Infrastructure;

public class AuthErrorResponse
{
    /// <summary>
    /// Описание возникшей ошибки
    /// </summary>
    string ExceptionMessage { get; set; }
    
    /// <summary>
    /// Код ошибки 
    /// </summary>
    /// <value> 1001 - Пользователь уже существует </value>
    /// <value> 1002 - Пароль отсутвует или слишком простой </value>
    /// <value> 1003 - Email отсутвует или имеет неверный формат </value>
    private int Code { get; set; }
}