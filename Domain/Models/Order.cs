using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class Order : Entity
{
    public DateTime CreatedAt { get; }
    public Guid CustomerId { get; }
    public Guid MenuId { get; }
    public Guid GroupId { get; set; }
    public OrderStatus Status { get; private set; }
    
    // Флаг, указывающий, что пользователь подтвердил, что перевел деньги за заказ администратору группы
    public bool Payment { get; private set; }

    // Включенные в заказ отдельные блюда
    public IReadOnlyCollection<OrderDish> Dishes => _orderDishes;
    private readonly List<OrderDish> _orderDishes;
    
    // Включенные в заказ комбо-обеды
    public IReadOnlyCollection<OrderLunchSet> LunchSets => _lunchSets;
    private readonly List<OrderLunchSet> _lunchSets;

    public Order(Guid customerId, Guid menuId, Guid groupId)
    {
        CustomerId = customerId;
        MenuId = menuId;
        CreatedAt = DateTime.UtcNow;
        GroupId = groupId;
        Status = OrderStatus.New;
        _orderDishes = new();
        _lunchSets = new();
    }

    public void AddDish(Menu menu, Guid menuDishId, int quantity)
    {
        if (quantity < 1)
            throw new DomainException("Попытка добавить блюдо с отрицательным количеством единиц");
        
        var dish = menu.Dishes.FirstOrDefault(x => x.Id == menuDishId);
        if (dish == null)
        {
            throw new DomainException(
                "Попытка добавить к заказу блюдо {DishId}, которого нет в меню {MenuId}",
                menuDishId, menu.Id);
        }
                                
        var orderUnit = new OrderDish(menuDishId, quantity);
        _orderDishes.Add(orderUnit);
    }

    public void ChangeDishQuantity(Guid menuDishId, int quantity)
    {
        var orderDishIndex = _orderDishes.FindIndex(x => x.MenuDishId == menuDishId);
        if (orderDishIndex == -1)
        {
            throw new DomainException(
                "Попытка изменить количество единиц блюда {DishId}, которого нет в заказе",
                menuDishId);
        }
        
        _orderDishes[orderDishIndex].ChangeQuantity(quantity);
        if (_orderDishes[orderDishIndex].Quantity == 0)
            _orderDishes.RemoveAt(orderDishIndex);
    }
    
    public void RemoveDish(Guid menuDishId)
    {
        var orderDishIndex = _orderDishes.FindIndex(x => x.MenuDishId == menuDishId);
        if (orderDishIndex == -1)
        {
            throw new DomainException(
                "Попытка удалить блюдо {DishId}, которого нет в заказе",
                menuDishId);
        }
        
        _orderDishes.RemoveAt(orderDishIndex);
    }

    /// <summary>
    /// Добавление комбо-набора к заказу. Имеет уникальный в рамках заказа идентификатор с типом int.
    /// Если в заказе есть комбо-набор с идентфикатором из меню, равным добавляемому, созадет еще один
    /// комбо-набор с новым внутренним идентификатором, увеличенным на 1.
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="menuLunchSetId"></param>
    /// <param name="options">Добавляемые опции к комбо-набору, где Key - id опции из меню, Value - количество единиц опций</param>
    /// <exception cref="DomainException"></exception>
    public void AddLunchSet(Menu menu, Guid menuLunchSetId, Dictionary<Guid, int> options)
    {
        var lunchSet = menu.LunchSets.FirstOrDefault(x => x.Id == menuLunchSetId);
        if (lunchSet == null)
        {
            throw new DomainException(
                "Попытка добавить к заказу комбо-набор {LunchSetId}, которого нет в меню {MenuId}",
                menuLunchSetId, menu.Id);
        }

        OrderLunchSet orderLunchSet;
        
        var orderLunchSetIndex = _lunchSets.FindIndex(x => x.MenuLunchSetId == menuLunchSetId);
        if (orderLunchSetIndex == -1)
        {
            orderLunchSet = new OrderLunchSet(1, menuLunchSetId);
            _lunchSets.Add(orderLunchSet);
        }
        else
        {
            var lastLunchSetWithSameId = _lunchSets.Where(x => x.MenuLunchSetId == menuLunchSetId).Max(x => x.InternalId);
            orderLunchSet = new OrderLunchSet(lastLunchSetWithSameId + 1, menuLunchSetId);
            _lunchSets.Add(orderLunchSet);
        }

        foreach (var option in options)
        {
            var menuOption = menu.Options.FirstOrDefault(x => x.Id == option.Key);
            if (menuOption == null)
            {
                throw new DomainException(
                    "Попытка добавить к заказу опцию {OptionId}, которой нет в меню {MenuId}",
                    option.Key, menu.Id);
            }
            orderLunchSet.AddOption(option.Key, option.Value);
        }
    }

    public void RemoveLunchSet(Guid menuLunchSetId, int id)
    {
        var orderLunchSetIndex = _lunchSets.FindIndex(x => x.MenuLunchSetId == menuLunchSetId && x.InternalId == id);
        if (orderLunchSetIndex == -1)
        {
            throw new DomainException(
                "Попытка удалить комбо-набор {DishId}, которого нет в заказе",
                menuLunchSetId);
        }
        
        _orderDishes.RemoveAt(orderLunchSetIndex);
    }

    /// <summary>
    /// Добавление опций к конкретному комбо-набору.
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="menuLunchSetId">Идентификатор комбо-набора из меню</param>
    /// <param name="internalLunchSetId">Внутренний идентификатор комбо-набора</param>
    /// <param name="options">Добавляемые опции к комбо-набору, где Key - id опции из меню, Value - количество единиц опций</param>
    /// <exception cref="DomainException"></exception>
    public void AddOptionsToLunchSet(Menu menu, Guid menuLunchSetId, int internalLunchSetId, Dictionary<Guid, int> options)
    {
        var menuLunchSetExists = menu.LunchSets.Any(x => x.Id == menuLunchSetId);
        if (!menuLunchSetExists)
        {
            throw new DomainException(
                "Попытка добавить к заказу комбо-набор {LunchSetId}, которого нет в меню {MenuId}",
                menuLunchSetId, menu.Id);
        }
        
        var orderLunchSetIndex = _lunchSets.FindIndex(x => x.MenuLunchSetId == menuLunchSetId && x.InternalId == internalLunchSetId);
        if (orderLunchSetIndex == -1)
        {
            throw new DomainException(
                "Комбо-набор {LunchSetId} {LunchSetInternalId} не найден в заказе",
                menuLunchSetId, internalLunchSetId);
        }
        
        var orderLunchSet = _lunchSets[orderLunchSetIndex];
        
        foreach (var option in options)
        {
            var menuOption = menu.Options.FirstOrDefault(x => x.Id == option.Key);
            if (menuOption == null)
            {
                throw new DomainException(
                    "Попытка добавить к заказу опцию {OptionId}, которой нет в меню {MenuId}",
                    option.Key, menu.Id);
            }
            orderLunchSet.AddOption(option.Key, option.Value);
        }
    }
    
    /// <summary>
    /// Удаление опций из комбо-набора
    /// </summary>
    /// <param name="menuLunchSetId">Идентификатор комбо-набора из меню</param>
    /// <param name="internalLunchSetId">Внутренний идентификатор комбо-набора</param>
    /// <param name="options">Идентификаторы опций из меню</param>
    /// <exception cref="DomainException"></exception>
    public void RemoveOptionsFromLunchSet(Guid menuLunchSetId, int internalLunchSetId, HashSet<Guid> options)
    {
        var orderLunchSetIndex = _lunchSets.FindIndex(x => x.MenuLunchSetId == menuLunchSetId && x.InternalId == internalLunchSetId);
        if (orderLunchSetIndex == -1)
        {
            throw new DomainException(
                "Комбо-набор {LunchSetId} {LunchSetInternalId} не найден в заказе",
                menuLunchSetId, internalLunchSetId);
        }
        
        var orderLunchSet = _lunchSets[orderLunchSetIndex];
        
        foreach (var menuOptionId in options)
        {
            orderLunchSet.RemoveOption(menuOptionId);
        }
    }

    public decimal GetTotalAmount(Menu menu)
    {
        var totalAmount = 0m;

        foreach (var dish in _orderDishes)
        {
            var menuDish = menu.Dishes.Single(x => x.Id == dish.MenuDishId);
            totalAmount += menuDish.Price * dish.Quantity;
        }

        foreach (var lunchSet in _lunchSets)
        {
            var menuLunchSet = menu.LunchSets.Single(x => x.Id == lunchSet.MenuLunchSetId);
            totalAmount += menuLunchSet.Price;

            foreach (var option in lunchSet.Options)
            {
                var menuOption = menu.Options.Single(x => x.Id == option.MenuOptionId);
                totalAmount += menuOption.Price * option.Quantity;
            }
        }

        return totalAmount;
    }

    public void ConfirmPayment()
    {
        Status = OrderStatus.Confirmed;
        Payment = true;
    }

    public void Cancel()
    {
        Status = OrderStatus.Canceled;
    }
    
    public enum OrderStatus
    {
        New,
        Confirmed,
        Canceled
    }
}

