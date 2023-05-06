using System.Collections.ObjectModel;
using Domain.Models.Base;

namespace Domain.Models;

public class DishType : Entity
{
    public string Name { get; private set; }
    public IReadOnlyCollection<Dish> Dishes { get; }

    public void ChangeName(string name)
    {
        Name = name;
    }
    
    public DishType(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Dishes = new Collection<Dish>();
    }

    private DishType() { }
}