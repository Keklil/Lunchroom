namespace Identity.Exceptions;

public class UnconfirmedEmailException : AuthException
{
    public UnconfirmedEmailException() : base("Почта не подтверждена.")
    {
    }
}