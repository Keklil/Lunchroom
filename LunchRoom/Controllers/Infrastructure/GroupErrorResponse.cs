namespace LunchRoom.Controllers.Infrastructure;

public class GroupErrorResponse
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
        UserIsMember = 1001,
        UserIsNotMember = 1002
    }
}
