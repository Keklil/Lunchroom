using Shared.DataTransferObjects.Menu;

namespace Shared.DataTransferObjects.Order;

public class OrderOptionDto
{
    public Guid Id { get; set; }
    public Guid OptionId { get; set; }
    public int OptionUnits { get; set; }
    public OptionDto Option { get; set; }
}