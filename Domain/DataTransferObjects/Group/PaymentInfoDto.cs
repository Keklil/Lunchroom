namespace Domain.DataTransferObjects.Group;

public class PaymentInfoDto
{
    public Guid GroupId { get; set; }
    public string Link { get; set; }
    public string? Description { get; set; }
    public string? Qr { get; set; }
}