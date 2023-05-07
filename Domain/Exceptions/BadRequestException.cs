using Domain.Exceptions.Base;
using Newtonsoft.Json;

namespace Domain.Exceptions;

public abstract class BadRequestException : StructuredException
{
    protected BadRequestException(string massageTemplate, params object[]? args)
        : base(massageTemplate, args)
    {
    }    
    
    protected BadRequestException(string massage)
        : base(massage)
    {
    }
}