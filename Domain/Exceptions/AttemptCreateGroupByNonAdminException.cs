namespace Domain.Exceptions;

public sealed class AttemptCreateGroupByNonAdminException : BadRequestException
{
    public AttemptCreateGroupByNonAdminException() : base("Attempt to create a group by a non-admin"){}
}