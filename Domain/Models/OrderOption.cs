using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class OrderOption : Entity
{
    private Option Option { get; }
    public Guid OptionId { get; }
    public int OptionUnits { get; }

    public decimal GetPrice()
    {
        return Option.Price;
    }
    
    public OrderOption(Option option, int optionUnits)
    {
        Id = Guid.NewGuid();
        OptionId = option.Id;
        Option = option;
        OptionUnits = optionUnits;
    }
    
    private OrderOption() { }
}