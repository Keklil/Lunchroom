using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class DomainException : StructuredException
{
    public DomainException(string messageTemplate, params object[]? args)
        : base(messageTemplate, args)
    {
    }
    
    public DomainException(string message)
        : base(message)
    {
    }
}