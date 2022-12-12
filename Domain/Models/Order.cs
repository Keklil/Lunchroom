namespace Domain.Models
{
    public class Order
    {
        public Guid Id { get; private set; }

        public DateTime OrderDate => _orderDate.ToLocalTime();
        private DateTime _orderDate;

        public Guid CustomerId => _customerId;
        private Guid _customerId;

        public Guid MenuId => _menuId;
        private Guid _menuId;
        
        public string OrderStatus { get; private set; }
        private int _orderStatusId;

        public bool Payment { get; private set; }
        
        public LunchSet LunchSet { get; private set; }
        public Guid LunchSetId => _lunchSetId;
        private Guid _lunchSetId;

        public IReadOnlyCollection<OrderOption> Options => _orderOptions;
        private List<OrderOption> _orderOptions = new();

        public Order(Guid customerId, Guid menuId)
        {
            Id = Guid.NewGuid();
            _customerId = customerId;
            _menuId = menuId;
            _orderDate = DateTime.UtcNow;
        }

        public void AddLunchSet(LunchSet lunchSet)
        {
            _lunchSetId = lunchSet.Id;
            LunchSet = lunchSet;
        }

        public void AddOption(Option option, int units)
        {

            var existingOption = _orderOptions.Where(o => o.OptionId == option.Id)
                .FirstOrDefault();

            if (existingOption == null)
            {
                var orderOption = new OrderOption(option, units);
                _orderOptions.Add(orderOption);
            }
        }

        public decimal GetTotal()
        {
            return LunchSet.Price + _orderOptions.Sum(o => o.GetPrice() * o.OptionUnits);
        }

        public decimal GetLunchSetPrice()
        {
            return LunchSet.Price;
        }

        public decimal[] GetOptionsPrice()
        {
            return Options.Select(x => x.GetPrice()).ToArray();
        }
        //Not usage
        public void ChangeStatus(int status)
        {
            OrderStatus = status.ToString();
        }

        public void ConfirmPayment()
        {
            Payment = true;
        }
    }
}
