namespace Domain.Exceptions;

public sealed class AttemptSetNullOrEmptyToken : Exception
{
    public AttemptSetNullOrEmptyToken() : base("Attempt to set null or empty token"){}
}