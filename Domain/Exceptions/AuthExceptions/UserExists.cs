namespace Domain.Exceptions.AuthExceptions;

public class UserExistsException : AuthException
{
    public UserExistsException() : base("User exists") {}
        
}