using Domain.Models.Base;

namespace Domain.Models;

public class PaymentInfo : ValueObject
{
    public string Link { get; }
    public string? Description { get; }
    public string? Qr { get; }
    
    public PaymentInfo( string link, string? description = null, string? qr = null)
    {
        Link = link;
        Description = description;
        Qr = qr;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Link;
    } 
    
    private PaymentInfo() { }
}