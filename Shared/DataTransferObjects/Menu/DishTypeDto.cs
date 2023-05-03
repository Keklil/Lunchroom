namespace Shared.DataTransferObjects.Menu;

public record DishTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}