using Entities.Exceptions;
using System;

namespace Entities.Models
{
    public class LunchSet
    {
        public Guid Id { get; set; }
        public decimal Price { get; private set; }
        public List<string> LunchSetList { get; private set; }

        public LunchSet(decimal price, List<string> lunchSetList)
        {
            Id = Guid.NewGuid();
            Price = price;
            LunchSetList = lunchSetList;
        }
        
    }
}
