using Domain.Exceptions;

namespace Identity.Exceptions;

public class AuthException : BadRequestException
{
    public AuthException(string massage) : base(massage)
    {
    }
}