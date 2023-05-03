namespace Shared.DataTransferObjects.Menu;

public class LunchSetDto
{
    public Guid Id { get; init; }
    public decimal Price { get; init; }
    public string? Name { get; init; }
    public List<DishDto>? Dishes { get; init; }
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
            Dishes = lunchSet.Dishes
                .Select(x => new DishDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Type = x.Type is not null ? new DishTypeDto() { Id = x.Type.Id, Name = x.Type.Name } : null
                })
                .ToList()
        };
    }
}