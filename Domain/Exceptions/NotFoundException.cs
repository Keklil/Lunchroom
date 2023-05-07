using Domain.Exceptions.Base;
using Domain.Models;

namespace Domain.Exceptions;

public class NotFoundException : StructuredException
{
    public NotFoundException(string massageTemplate, params object[]? values)
        : base(massageTemplate, values)
    {
    }
    
    public NotFoundException(string massage)
        : base(massage)
    {
    }
}