namespace Shared.DataTransferObjects.Menu;

public class MenuDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<LunchSetDto> LunchSets { get; set; }
    public List<OptionDto> Options { get; set; }
}

public static class MenuDtoMapper
{
    public static MenuDto Map(this Domain.Models.Menu menu)
    {
        return new MenuDto
        {
            Id = menu.Id,
            Date = menu.Date,
            LunchSets = menu.LunchSets.Select(lunchSet => lunchSet.Map()).ToList(),
            Options = menu.Options.Select(option => option.Map()).ToList()
        };
    }
}