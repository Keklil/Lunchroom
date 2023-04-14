namespace Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string massage, params string[]? values) : base(massage)
    {
    }
}