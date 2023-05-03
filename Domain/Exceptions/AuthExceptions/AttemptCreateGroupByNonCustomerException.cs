namespace Domain.Exceptions.AuthExceptions;

public sealed class AttemptCreateGroupByNonCustomerException : BadRequestException
{
    public AttemptCreateGroupByNonCustomerException() : base("Попытка создать группу пользователем без роли Customer")
    {
    }
}