namespace Domain.DataTransferObjects.Menu
{
    public class LunchSetDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public List<string>? LunchSetList { get; set; }
    }
}
