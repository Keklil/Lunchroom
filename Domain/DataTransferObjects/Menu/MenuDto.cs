namespace Domain.DataTransferObjects.Menu;

public class MenuDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<LunchSetDto> LunchSets { get; set; }
    public List<OptionDto> Options { get; set; }
}