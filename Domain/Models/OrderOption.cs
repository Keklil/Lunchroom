using Domain.Exceptions;

namespace Domain.Models
{
    public class OrderOption
    {
        public Guid Id { get; private set; }
        private Option Option { get; set; }
        public Guid OptionId => _optionId;
        private Guid _optionId;

        public int OptionUnits => _optionUnits;
        private int _optionUnits;

        private OrderOption(int optionUnits)
        {
            Id = Guid.NewGuid();
            _optionUnits = optionUnits;
        }
        public OrderOption(Option option, int optionUnits)
        {
            Id = Guid.NewGuid();
            _optionId = option.Id;
            Option = option;
            _optionUnits = optionUnits;
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new DomainException("Invalid units");
            }
            _optionUnits += units;
        }

        public decimal GetPrice()
        {
            return Option.Price;
        }
    }
}
