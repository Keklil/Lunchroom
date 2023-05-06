using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class Order : Entity
{
    public DateTime OrderDate { get; }
    public Guid CustomerId { get; }
    public Guid MenuId { get; }
    public Guid GroupId { get; set; }
    public OrderStatus Status { get; private set; }
    public bool Payment { get; private set; }
    public LunchSet? LunchSet { get; private set; }
    public int LunchSetUnits { get; private set; }
    public IReadOnlyCollection<OrderOption> Options => _orderOptions;
    private readonly List<OrderOption> _orderOptions = new();

    public Order(Guid customerId, Guid menuId, Guid groupId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        MenuId = menuId;
        OrderDate = DateTime.UtcNow;
        GroupId = groupId;
        Status = OrderStatus.New;
    }

    public void AddLunchSet(LunchSet lunchSet, int units)
    {
        if (units < 1)
            throw new DomainException("Попытка установить некорректное количество");
        
        LunchSet = lunchSet;
        LunchSetUnits = units;
    }

    public void AddOption(Option option, int units)
    {
        if (units < 1)
            throw new DomainException("Попытка установить некорректное количество");

        var existingOption = _orderOptions
            .FirstOrDefault(o => o.OptionId == option.Id);

        if (existingOption == null)
        {
            var orderOption = new OrderOption(option, units);
            _orderOptions.Add(orderOption);
        }
    }

    public decimal GetTotal()
    {
        var lunchSetPrice = LunchSet?.Price ?? 0;
        return lunchSetPrice + _orderOptions.Sum(o => o.GetPrice() * o.OptionUnits);
    }

    public decimal[] GetOptionsPrice()
    {
        return Options.Select(x => x.GetPrice()).ToArray();
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
}

public enum OrderStatus
{
    New,
    Confirmed,
    Canceled
}