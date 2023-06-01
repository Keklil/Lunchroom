namespace Shared.DataTransferObjects.Order;

public static class OrderMapper
{
    public static OrderDto Map(this Domain.Models.Order source, Domain.Models.Menu menu)
    {
        return new OrderDto
        {
            Id = source.Id,
            CustomerId = source.CustomerId,
            GroupId = source.GroupId,
            OrderDate = source.CreatedAt,
            LunchSets = source.LunchSets.Select(x => new OrderLunchSetDto
            {
                Id = x.MenuLunchSetId,
                Price = menu.LunchSets.First(l => l.Id == x.MenuLunchSetId).Price,
                InternalId = x.InternalId,
                Options = x.Options.Select(o => new OrderOptionDto
                {
                    Id = o.MenuOptionId,
                    Quantity = o.Quantity,
                    Price = menu.Options.First(m => m.Id == o.MenuOptionId).Price,
                    Name = menu.Options.First(m => m.Id == o.MenuOptionId).Name,
                    DishId = menu.Options.First(m => m.Id == o.MenuOptionId).Dish.Id,
                }).ToList()
            }).ToList(),
            Dishes = source.Dishes.Select(sourceOption => new OrderDishDto()
            {
                Id = sourceOption.MenuDishId,
                Name = menu.Dishes.First(dish => dish.Id == sourceOption.MenuDishId).Name,
                Price = menu.Dishes.First(dish => dish.Id == sourceOption.MenuDishId).Price,
                Quantity = sourceOption.Quantity,
            }).ToList(),
            Payment = source.Payment
        };
    }
}