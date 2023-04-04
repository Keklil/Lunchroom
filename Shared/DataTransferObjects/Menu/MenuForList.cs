namespace Shared.DataTransferObjects.Menu;

public class MenuForList
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
}

public static class MenuForListMapper
{
    public static MenuForList MapToMenuForList(this Domain.Models.Menu menu)
    {
        return new MenuForList
        {
            Id = menu.Id,
            Date = menu.Date
        };
    }
    
}