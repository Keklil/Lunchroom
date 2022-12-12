namespace Domain.SecurityModels;

public class EmailValidation
{
    public Guid Id { get; private set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public EmailValidation(string email, string token)
    {
        Email = email;
        Token = token;
    }
}