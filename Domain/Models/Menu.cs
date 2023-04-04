using System;
using Domain.Exceptions;

namespace Domain.Models
{
    public class Menu
    {
        public Guid Id { get; set; }
        public DateTime Date { get; }
        public Guid GroupId { get; }
        public IReadOnlyCollection<LunchSet> LunchSets => _lunchSets;
        private List<LunchSet> _lunchSets;

        public IReadOnlyCollection<Option> Options => _options;
        private List<Option> _options;

        public bool IsReported { get; private set; }

        private Menu(){}
        
        public Menu(Guid groupId)
        {
            Id = Guid.NewGuid();
            Date = DateTime.UtcNow;
            GroupId = groupId;
            _lunchSets = new List<LunchSet>();
            _options = new List<Option>();
            IsReported = false;
        }

        public void AddLunchSet(decimal price, List<string> lunchSet)
        {
            if (lunchSet is null || lunchSet.Count == 0)
            {
                throw new DomainException("List of lunch set items is empty");
            }
            LunchSet newLunchSet = new LunchSet(price, lunchSet);
            _lunchSets.Add(newLunchSet);
        }

        public void AddOption(string name, decimal price)
        {
            if (price < 0)
            {
                throw new DomainException("Invalid price");
            }
            Option newOption = new Option(name, price);
            _options.Add(newOption);
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
    }
}
