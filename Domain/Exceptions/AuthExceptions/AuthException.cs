namespace Domain.Exceptions.AuthExceptions;

public class AuthException : BadRequestException
{
    public AuthException(string massage) : base(massage)
    {
    }
}