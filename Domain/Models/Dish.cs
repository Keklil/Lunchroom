using Domain.Models.Base;

namespace Domain.Models;

public class Dish : Entity
{
    public string Name { get; }
    public decimal Price { get; }
    public DishType? Type { get; }

    public Dish(string name, decimal price, DishType? type = null)
    {
        Name = name;
        Price = price;
        Type = type;
    }
    
    private Dish() { }
}