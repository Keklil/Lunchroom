namespace Domain.Exceptions;

public sealed class AttemptCreateGroupByNonAdmin : Exception
{
    public AttemptCreateGroupByNonAdmin() : base("Attempt to create a group by a non-admin"){}
}