﻿using Domain.Models.Base;

namespace Domain.Models;

public class Option : Entity
{
    public string? Name { get; }
    public decimal Price { get; }
    public Dish Dish { get; }

    public Option(Dish dish, string? name = null, decimal? price = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price ?? dish.Price;
        Dish = dish;
    }
    
    private Option() { }
}