namespace Shared.DataTransferObjects.Menu;

public class MenuForCreationDto
{
    public List<LunchSetForCreationDto> LunchSets { get; set; }
    public List<OptionForCreationDto> Options { get; set; }
}