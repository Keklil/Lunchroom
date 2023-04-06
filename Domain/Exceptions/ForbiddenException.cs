namespace Domain.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Недостаточно прав для доступа к ресурсу")
    {
    }
}