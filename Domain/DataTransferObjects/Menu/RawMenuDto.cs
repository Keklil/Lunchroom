namespace Domain.DataTransferObjects.Menu;

public record RawMenuDto
{
    public List<string> Menu { get; set; }
    public Guid GroupId { get; set; }
};