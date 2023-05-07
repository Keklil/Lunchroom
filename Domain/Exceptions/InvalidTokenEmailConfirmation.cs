namespace Domain.Exceptions;

public class InvalidTokenEmailConfirmation : BadRequestException
{
    public InvalidTokenEmailConfirmation() : base("Неверный токен подтверждения почты")
    {
    }
}