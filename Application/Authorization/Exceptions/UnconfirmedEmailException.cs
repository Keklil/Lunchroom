namespace Application.Authorization.Exceptions;

public class UnconfirmedEmailException : AuthException
{
    public UnconfirmedEmailException() : base("Почта не подтверждена.")
    {
    }
}