namespace Domain.Exceptions;

public class InvalidTokenEmailConfirmation : BadRequestException
{
    public InvalidTokenEmailConfirmation() : base("Invalid email confirmation token")
    {
    }
}