namespace Entities.Exceptions;

public class InvalidTokenEmailConfirmation : BadRequestException
{
    public InvalidTokenEmailConfirmation() : base("Ivalid email confirmation token") { }
}