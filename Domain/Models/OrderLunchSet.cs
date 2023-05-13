using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class OrderLunchSet : ValueObject
{
    public int InternalId { get; }
    public Guid MenuLunchSetId { get; }
    public IReadOnlyCollection<OrderOption> Options => _options;
    private readonly List<OrderOption> _options;
    
    public OrderLunchSet(int internalId, Guid menuLunchSetId)
    {
        InternalId = internalId;
        MenuLunchSetId = menuLunchSetId;
        _options = new();
    }
    
    /// <summary>
    /// Добавление опции к комбо-набору. Если опция уже есть, то заменяет ее количество
    /// </summary>
    /// <param name="menuOptionId">Идентификатор опции в меню</param>
    /// <param name="quantity">Количество единиц опции</param>
    public void AddOption(Guid menuOptionId, int quantity)
    {
        var existingOptionIndex = _options
            .FindIndex(o => o.MenuOptionId.Equals(menuOptionId));

        if (existingOptionIndex == -1)
        {
            var orderOption = new OrderOption(menuOptionId, quantity);
            _options.Add(orderOption);
        }
        else
        {
            _options[existingOptionIndex].ChangeQuantity(quantity);
        }
    }

    public void RemoveOption(Guid menuOptionId)
    {
        var existingOptionIndex = _options
            .FindIndex(o => o.MenuOptionId.Equals(menuOptionId));

        if (existingOptionIndex == -1)
        {
            _options.RemoveAt(existingOptionIndex);
        }
        else
        {
            throw new DomainException("Попытка удалить опцию, которая не была добавлена в комбо-набор");
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return InternalId;
    }
    
    private OrderLunchSet() { }
}