namespace LunchRoom.Controllers.Infrastructure;

public class GroupErrorResponse
{
    public enum ErrorCodes
    {
        UserIsMember = 1001,
        UserIsNotMember = 1002,
        AttemptCreateGroupByNonAdmin = 1003
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