using Domain.Models.Base;

namespace Domain.Models;

public class Contacts : ValueObject
{
    public string? Email { get; }
    public string? Phone { get; }

    public Contacts(string? email, string? phone)
    {
        Email = email;
        Phone = phone;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
        yield return Phone;
    }
    
    private Contacts(){}
}