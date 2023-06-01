using Shared.DataTransferObjects.Menu;

namespace Shared.DataTransferObjects.Order;

public class OrderOptionDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public Guid DishId { get; set; }
}