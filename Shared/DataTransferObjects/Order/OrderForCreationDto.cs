namespace Shared.DataTransferObjects.Order;

public class OrderForCreationDto
{
    public Guid GroupId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid MenuId { get; set; }
    public Guid LunchSetId { get; set; }
    public int LunchSetUnits { get; set; }
    public List<OrderOptionForCreationDto> Options { get; set; }
}