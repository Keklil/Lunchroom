using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class OrderOption : ValueObject
{
    public Guid MenuOptionId { get; }
    public int Quantity { get; private set; }
    
    public void ChangeQuantity(int quantity)
    {
        if (quantity < 1)
            throw new DomainException("Попытка установить недопустимое количесво единиц опции");
        
        Quantity = quantity;
    }
    
    public OrderOption(Guid menuUnitId, int quantity)
    {
        MenuOptionId = menuUnitId;
        Quantity = quantity;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MenuOptionId;
    }
    
    private OrderOption() { }
}