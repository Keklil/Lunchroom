namespace Domain.Exceptions.AuthExceptions;

public class UnconfirmedEmailException : AuthException
{
    public UnconfirmedEmailException() : base("Почта не подтверждена.")
    {
    }
}