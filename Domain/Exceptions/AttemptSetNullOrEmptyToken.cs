namespace Domain.Exceptions;

public sealed class AttemptSetNullOrEmptyTokenException : Exception
{
    public AttemptSetNullOrEmptyTokenException() : base("Attempt to set null or empty token")
    {
    }
}