using Domain.DataTransferObjects.Menu;

namespace Domain.DataTransferObjects.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid GroupId { get; set; }
        public DateTime OrderDate { get; set; }
        public LunchSetDto LunchSet { get; set; }
        public List<OrderOptionDto> Options { get; set; }
        public bool Payment { get; set; }

    }
}
