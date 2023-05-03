using System.Collections.ObjectModel;

namespace Domain.Models;

public class DishType
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public IReadOnlyCollection<Dish> Dishes { get; }

    public void ChangeName(string name)
    {
        Name = name;
    }
    
    public DishType(string name)
    {
        Name = name;
        Dishes = new Collection<Dish>();
    }

    private DishType() { }
}