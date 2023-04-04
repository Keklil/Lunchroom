namespace Shared.DataTransferObjects.Order;

public static class OrderMapper
{
    public static OrderDto Map(this Domain.Models.Order source)
    {
        return new OrderDto
        {
            Id = source.Id,
            CustomerId = source.CustomerId,
            GroupId = source.GroupId,
            OrderDate = source.OrderDate,
            LunchSet = source.LunchSetId != default
                ? new OrderLunchSetDto
                {
                    Id = source.LunchSet.Id,
                    Price = source.LunchSet.Price,
                    LunchSetList = source.LunchSet.LunchSetList,
                    LunchSetUnits = source.LunchSetUnits
                }
                : null,
            Options = source.Options.Select(sourceOption => new OrderOptionDto
            {
                Id = sourceOption.Id,
                OptionId = sourceOption.OptionId,
                OptionUnits = sourceOption.OptionUnits
            }).ToList(),
            Payment = source.Payment
        };
    }
}