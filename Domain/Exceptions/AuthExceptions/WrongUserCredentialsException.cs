namespace Domain.Exceptions.AuthExceptions;

public class WrongUserCredentialsException : AuthException
{
    public WrongUserCredentialsException() : base("Почта и пароль не совпадают."){}
}