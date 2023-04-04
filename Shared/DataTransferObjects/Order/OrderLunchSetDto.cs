namespace Shared.DataTransferObjects.Order;

public class OrderLunchSetDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public List<string>? LunchSetList { get; set; }
    public int LunchSetUnits { get; set; }
}