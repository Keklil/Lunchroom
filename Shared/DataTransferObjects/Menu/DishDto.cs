using Domain.Models;

namespace Shared.DataTransferObjects.Menu;

public record DishDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public DishTypeDto? Type { get; init; }
}

public static class DishMapper
{
    public static DishDto Map(this Dish dish)
    {
        return new DishDto
        {
            Id = dish.Id,
            Name = dish.Name,
            Price = dish.Price,
            Type = dish.Type?.Map()
        };
    }
}