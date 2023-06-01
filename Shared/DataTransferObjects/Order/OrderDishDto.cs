namespace Shared.DataTransferObjects.Order;

public record OrderDishDto
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public string Name { get; init; }
}