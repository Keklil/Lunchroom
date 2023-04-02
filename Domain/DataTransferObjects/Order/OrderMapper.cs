using Domain.DataTransferObjects.Menu;

namespace Domain.DataTransferObjects.Order;

public static class OrderMapper
{
    public static OrderDto Map(this Models.Order source)
    {
        return new OrderDto
        {
            Id = source.Id,
            CustomerId = source.CustomerId,
            GroupId = source.GroupId,
            OrderDate = source.OrderDate,
            LunchSet = source.LunchSetId != default
                ? new LunchSetDto
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