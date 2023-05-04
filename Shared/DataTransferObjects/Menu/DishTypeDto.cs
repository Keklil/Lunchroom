namespace Shared.DataTransferObjects.Menu;

public record DishTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public static class DishTypeMapper
{
    public static DishTypeDto Map(this Domain.Models.DishType type)
    {
        return new DishTypeDto
        {
            Id = type.Id,
            Name = type.Name
        };
    }
}