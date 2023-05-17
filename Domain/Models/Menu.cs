﻿using Domain.Exceptions;
using Domain.Models.Base;
using Domain.Notifications;

namespace Domain.Models;

public class Menu : Entity
{
    public DateTime CreatedAt { get; }
    public Guid KitchenId { get; }
    public bool IsReported { get; private set; }
    public bool IsPublished { get; private set; }
    
    public IReadOnlyCollection<LunchSet> LunchSets => _lunchSets;
    private readonly List<LunchSet> _lunchSets;

    public IReadOnlyCollection<Option> Options => _options;
    private readonly List<Option> _options;
        
    public IReadOnlyCollection<Dish> Dishes => _dishes;
    private readonly List<Dish> _dishes;

    public Menu(Guid kitchenId)
    {
        CreatedAt = DateTime.UtcNow;
        KitchenId = kitchenId;
        _lunchSets = new List<LunchSet>();
        _options = new List<Option>();
        _dishes = new List<Dish>();
        IsReported = false;
    }

    public void AddLunchSet(List<Dish> dishes, decimal price, string? name = null)
    {
        var dishIds = Dishes.Select(x => x.Id).ToHashSet();
        foreach (var dish in dishes)
        {
            if (!dishIds.Contains(dish.Id))
                throw new DomainException("Попытка добавить в меню ланч-сет с блюдом ({DishId}), не принадлежащем к меню {MenuId}", dish.Id, Id);
        }
            
        if (price < 0)
            throw new DomainException("Попытка установить отрицательную цену для опции");

        var newLunchSet = new LunchSet(dishes, price, name);
        _lunchSets.Add(newLunchSet);
    }

    public void AddOption(Dish dish, bool allowMultipleSelection, string? name = null, decimal? price = null)
    {
        var dishIds = Dishes.Select(x => x.Id).ToHashSet();
        if (!dishIds.Contains(dish.Id))
            throw new DomainException("Попытка добавить в меню опцию с блюдом ({DishId}), не принадлежащем к меню {MenuId}", dish.Id, Id);
            
        if (price < 0)
            throw new DomainException("Попытка установить отрицательную цену для опции");

            
        var newOption = new Option(dish, allowMultipleSelection, name, price);
        _options.Add(newOption);
    }

    public void AddDish(string name, decimal price, DishType? type = null)
    {
        if (price < 0)
            throw new DomainException("Попытка установить отрицательную цену для блюда");

        var newDish = new Dish(name, price, type);
        _dishes.Add(newDish);
    }

    public LunchSet? GetLunchSetById(Guid lunchSetId)
    {
        return LunchSets.SingleOrDefault(lunchSet => lunchSet.Id == lunchSetId);
    }

    public Option? GetOptionById(Guid optionId)
    {
        return Options.SingleOrDefault(option => option.Id == optionId);
    }
    
    public void Publish()
    {
        if (DateTime.UtcNow.Date != CreatedAt.Date)
            throw new DomainException(
                "Попытка опубликовать меню с днем загрузки {MenuCreationDate}, не совпадающим с текущей датой {PublishDate}",
                CreatedAt, DateTime.UtcNow);
            
        IsPublished = true;
        AddDomainEvent(new MenuPublished(Id, KitchenId));
    }

    public void Reported()
    {
        IsReported = true;
    }
        
    private Menu(){}
}