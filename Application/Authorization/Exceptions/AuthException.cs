using Domain.Exceptions;

namespace Application.Authorization.Exceptions;

public class AuthException : BadRequestException
{
    public AuthException(string massage) : base(massage)
    {
    }
}