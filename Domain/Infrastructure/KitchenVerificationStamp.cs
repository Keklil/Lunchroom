using Domain.Models;

namespace Domain.Infrastructure;

public class KitchenVerificationStamp
{
    public Guid Id { get; }
    public DateTimeOffset VerificationTimestamp { get; }
    public User Checker { get; }

    public KitchenVerificationStamp(User checker)
    {
        Id = Guid.NewGuid();
        VerificationTimestamp = DateTimeOffset.UtcNow;
        Checker = checker;
    }
    
    private KitchenVerificationStamp() { }
}