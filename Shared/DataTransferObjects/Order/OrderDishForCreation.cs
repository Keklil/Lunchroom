namespace Shared.DataTransferObjects.Order;

public record OrderDishForCreation
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
}