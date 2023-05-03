namespace Shared;

public class ImportReport
{
    private string _error;
    private readonly List<BadRow> _errorsInRows = new();

    public IReadOnlyList<BadRow> ErrorsInRows => _errorsInRows;
    public ImportStatus Status { get; set; } = ImportStatus.Success;
        
    public string ErrorsWhileImport
    {
        get => _error;
        set
        {
            _error = value;
            Status = ImportStatus.Error;
        }
    }

    public void AddBadRowError(BadRow badRow)
    {
        _errorsInRows.Add(badRow);
        Status = ImportStatus.Partial;
    }
        
    public class BadRow
    {
        public int Number { get; set; }
        public List<string> Errors { get; } = new();
    }
}
    
public enum ImportStatus
{
    Success,
    Partial,
    Error
}