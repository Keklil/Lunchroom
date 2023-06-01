namespace Shared.DataTransferObjects.Order;

public record OrderForCreationDto
{
    public Guid GroupId { get; init; }
    public Guid MenuId { get; init; }
    public List<OrderLunchSetForCreation> LunchSets { get; init; } = new();
    public List<OrderDishForCreation> Dishes { get; init; } = new();
}