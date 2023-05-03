using Domain.Models;

namespace Shared.DataTransferObjects.Menu;

public record DishDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public DishTypeDto? Type { get; init; }
}