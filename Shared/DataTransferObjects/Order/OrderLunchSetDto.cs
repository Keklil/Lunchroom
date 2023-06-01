namespace Shared.DataTransferObjects.Order;

public class OrderLunchSetDto
{
    public Guid Id { get; init; }
    public decimal Price { get; init; }
    public int InternalId { get; init; }
    public List<OrderOptionDto> Options { get; init; } = new();
}

