using Shared.DataTransferObjects.Menu;

namespace Shared.DataTransferObjects.Order;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid GroupId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderLunchSetDto? LunchSet { get; set; }
    public List<OrderOptionDto> Options { get; set; } = new();
    public bool Payment { get; set; }
}