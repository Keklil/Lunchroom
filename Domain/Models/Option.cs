using Domain.Models.Base;

namespace Domain.Models;

public class Option : Entity
{
    public string Name => _name ?? Dish.Name;
    private string? _name;
    public decimal Price { get; }
    public Dish Dish { get; }
    public bool AllowMultipleSelectionInOneLunchSet { get; private set; }
    
    public void ChangeName(string name)
    {
        _name = name;
    }

    public void AllowMultipleSelection()
    {
        AllowMultipleSelectionInOneLunchSet = true;
    }
    
    public void DisallowMultipleSelection()
    {
        AllowMultipleSelectionInOneLunchSet = false;
    }

    public Option(Dish dish, bool allowMultipleSelectionInOneLunchSet, string? name = null, decimal? price = null)
    {
        _name = name;
        AllowMultipleSelectionInOneLunchSet = allowMultipleSelectionInOneLunchSet;
        Price = price ?? dish.Price;
        Dish = dish;
    }
    
    private Option() { }
}