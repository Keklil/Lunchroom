namespace Entities.DataTransferObjects
{
    public class OrderForCreationDto
    {
        public Guid CustomerId { get; set; }
        public Guid MenuId { get; set; }
        public Guid LunchSetId { get; set; }
        public List<OrderOptionForCreationDto> Options { get; set; }

    }
}
