using Domain.Models.Base;

namespace Domain.Models;

public class Dish
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public DishType? Type { get; }

    public Dish(string name, decimal price, DishType? type = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Type = type;
    }
    
    private Dish() { }
}