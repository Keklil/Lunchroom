namespace Domain.Exceptions.AuthExceptions;

public sealed class AttemptCreateGroupByNonAdminException : BadRequestException
{
    public AttemptCreateGroupByNonAdminException() : base("Attempt to create a group by a non-admin")
    {
    }
}