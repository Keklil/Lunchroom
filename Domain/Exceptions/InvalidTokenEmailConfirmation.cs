namespace Domain.Exceptions;

public class InvalidTokenEmailConfirmationException : BadRequestException
{
    public InvalidTokenEmailConfirmationException() : base("Неверный токен подтверждения почты")
    {
    }
}