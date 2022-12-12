namespace Domain.DataTransferObjects.Menu
{
    public class LunchSetForCreationDto
    {
        public decimal Price { get; set; }
        public List<string> LunchSetList { get; set; } = null!;
    }
}
