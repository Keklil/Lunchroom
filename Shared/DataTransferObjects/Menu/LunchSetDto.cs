namespace Shared.DataTransferObjects.Menu;

public class LunchSetDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public List<string>? LunchSetList { get; set; }
}

public static class LunchSetMapper
{
    public static LunchSetDto Map(this Domain.Models.LunchSet lunchSet)
    {
        return new LunchSetDto
        {
            Id = lunchSet.Id,
            Price = lunchSet.Price,
            LunchSetList = lunchSet.LunchSetList,
        };
    }
}