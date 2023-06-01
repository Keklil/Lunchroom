using Shared.DataTransferObjects.Menu;

namespace Shared.DataTransferObjects.Order;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid GroupId { get; set; }
    public Guid MenuId { get; }
    public DateTime OrderDate { get; set; }
    public bool Payment { get; set; }
    public decimal TotalAmount => Dishes.Sum(x => x.Price * x.Quantity) + 
                                  LunchSets.Sum(x => x.Price + x.Options.Sum(o => o.Price * o.Quantity));
    public List<OrderDishDto> Dishes { get; set; } = new();
    public List<OrderLunchSetDto> LunchSets { get; set; } = new();
}