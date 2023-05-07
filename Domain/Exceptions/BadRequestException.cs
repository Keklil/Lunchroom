using Domain.Exceptions.Base;
using Newtonsoft.Json;

namespace Domain.Exceptions;

public abstract class BadRequestException : StructuredException
{
    protected BadRequestException(string messageTemplate, params object[]? args)
        : base(messageTemplate, args)
    {
    }    
    
    protected BadRequestException(string massage)
        : base(massage)
    {
    }
}