namespace Shared.DataTransferObjects.Menu;

public class MenuForHistoryDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
}

public static class MenuForListMapper
{
    public static MenuForHistoryDto MapToMenuForList(this Domain.Models.Menu menu)
    {
        return new MenuForHistoryDto
        {
            Id = menu.Id,
            Date = menu.CreatedAt
        };
    }
    
}