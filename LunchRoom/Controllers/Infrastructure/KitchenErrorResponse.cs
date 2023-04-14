namespace LunchRoom.Controllers.Infrastructure;

public class KitchenErrorResponse
{
    public enum ErrorCodes
    {
        UserIsMember = 2001,
        AttemptCreateGroupByNonKitchenOperator = 2002
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