namespace Domain.Models;

public class PaymentInfo
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public byte[] Qr { get; set; }
}