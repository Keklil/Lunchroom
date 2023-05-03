using Domain.Exceptions;
using System;

namespace Domain.Models
{
    public class LunchSet
    {
        public Guid Id { get; set; }
        public decimal Price { get; }
        public string? Name { get; }
        public IReadOnlyCollection<Dish> Dishes { get; }

        public LunchSet(decimal price, List<Dish> dishes, string? name = null)
        {
            Id = Guid.NewGuid();
            Price = price;
            Dishes = dishes;
            Name = name;
        }
        
        private LunchSet() { }
    }
}
