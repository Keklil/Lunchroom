namespace Shared.DataTransferObjects.Menu;

public class LunchSetDto
{
    public Guid Id { get; init; }
    public decimal Price { get; init; }
    public string? Name { get; init; }
    public List<Guid>? DishIds { get; init; }
}

public static class LunchSetMapper
{
    public static LunchSetDto Map(this Domain.Models.LunchSet lunchSet)
    {
        return new LunchSetDto
        {
            Id = lunchSet.Id,
            Name = lunchSet.Name,
            Price = lunchSet.Price,
            DishIds = lunchSet.Dishes.Select(x => x.Id).ToList()
        };
    }
}