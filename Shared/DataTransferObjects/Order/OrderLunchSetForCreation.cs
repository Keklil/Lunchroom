namespace Shared.DataTransferObjects.Order;

public record OrderLunchSetForCreation
{
    public Guid Id { get; init; }
    public List<OrderOptionForCreationDto> Options { get; init; } = new();
}