namespace Identity.Exceptions;

public class WrongUserCredentialsException : AuthException
{
    public WrongUserCredentialsException() : base("Почта и пароль не совпадают.")
    {
    }
}