using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class OrderDish : ValueObject
{
    public Guid MenuDishId { get; }
    public int Quantity { get; private set; }
    
    public void ChangeQuantity(int quantity)
    {
        var tempQuantity = Quantity + quantity;
        if (tempQuantity < 1)
            throw new DomainException("Недопустимое итоговое количество единиц блюда");
        
        Quantity = tempQuantity;
    }
    
    public OrderDish(Guid menuDishId, int quantity)
    {
        MenuDishId = menuDishId;
        Quantity = quantity;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MenuDishId;
    }
    
    private OrderDish() { }
}