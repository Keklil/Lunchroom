namespace Domain.Exceptions.Base;

public abstract class StructuredException : Exception
{
    public string? MessageTemplate { get; }
    public object[] Args { get; }

    protected StructuredException(string messageTemplate, params object[] args)
    {
        MessageTemplate = messageTemplate;
        Args = args;
    }
    
    protected StructuredException(string message)
        : base(message)
    {
        Args = Array.Empty<object>();
    }
}