﻿using System;
using Domain.Exceptions;
using Domain.Models.Base;

namespace Domain.Models;

public class Menu : Entity
{
    public DateTime Date { get; }
    public Guid KitchenId { get; }
    public IReadOnlyCollection<LunchSet> LunchSets => _lunchSets;
    private readonly List<LunchSet> _lunchSets;

    public IReadOnlyCollection<Option> Options => _options;
    private readonly List<Option> _options;
        
    public List<Dish> Dishes { get; }
    public bool IsReported { get; private set; }

    public Menu(Guid kitchenId)
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
        KitchenId = kitchenId;
        _lunchSets = new List<LunchSet>();
        _options = new List<Option>();
        Dishes = new List<Dish>();
        IsReported = false;
    }

    public void AddLunchSet(decimal price, List<Dish> dishes)
    {
        var dishIds = Dishes.Select(x => x.Id).ToHashSet();
        foreach (var dish in dishes)
        {
            if (!dishIds.Contains(dish.Id))
                throw new DomainException($"Попытка добавить в меню ланч-сет с блюдом ({dish.Id}), не принадлежащем к меню");
        }
            
        if (price < 0)
            throw new DomainException("Попытка установить отрицательную цену для опции");

        var newLunchSet = new LunchSet(price, dishes);
        _lunchSets.Add(newLunchSet);
    }

    public void AddOption(Dish dish, string? name = null, decimal? price = null)
    {
        var dishIds = Dishes.Select(x => x.Id).ToHashSet();
        if (!dishIds.Contains(dish.Id))
            throw new DomainException($"Попытка добавить в меню опцию с блюдом ({dish.Id}), не принадлежащем к меню");
            
        if (price < 0)
            throw new DomainException("Попытка установить отрицательную цену для опции");

            
        var newOption = new Option(dish, name, price);
        _options.Add(newOption);
    }

    public void AddDish(string name, decimal price, DishType? type = null)
    {
        if (price < 0)
            throw new DomainException("Попытка установить отрицательную цену для блюда");

        var newDish = new Dish(name, price, type);
        Dishes.Add(newDish);
    }

    public LunchSet? GetLunchSetById(Guid lunchSetId)
    {
        return LunchSets.SingleOrDefault(lunchSet => lunchSet.Id == lunchSetId);
    }

    public Option? GetOptionById(Guid optionId)
    {
        return Options.SingleOrDefault(option => option.Id == optionId);
    }

    public void Reported()
    {
        IsReported = true;
    }
        
    private Menu(){}
}