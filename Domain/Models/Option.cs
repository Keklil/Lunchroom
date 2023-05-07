using Domain.Models.Base;

namespace Domain.Models;

public class Option : Entity
{
    public string Name => _name ?? Dish.Name;
    private string? _name;
    public decimal Price { get; }
    public Dish Dish { get; }
    
    public void ChangeName(string name)
    {
        _name = name;
    }

    public Option(Dish dish, string? name = null, decimal? price = null)
    {
        _name = name;
        Price = price ?? dish.Price;
        Dish = dish;
    }
    
    private Option() { }
}