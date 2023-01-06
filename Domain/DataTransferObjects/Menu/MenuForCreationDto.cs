namespace Domain.DataTransferObjects.Menu
{
    public class MenuForCreationDto
    {
        public Guid GroupId { get; set; }
        public List<LunchSetForCreationDto> LunchSets { get; set; }
        public List<OptionForCreationDto> Options { get; set; }
    }
}
