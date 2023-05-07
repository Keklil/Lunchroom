using Domain.Exceptions;
using System;
using Domain.Models.Base;

namespace Domain.Models;

public class LunchSet : Entity
{
    public decimal Price { get; }
    public string? Name { get; }
    public IReadOnlyCollection<Dish> Dishes { get; }

    public LunchSet(List<Dish> dishes, decimal price, string? name = null)
    {
        Price = price;
        Dishes = dishes;
        Name = name;
    }
        
    private LunchSet() { }
}