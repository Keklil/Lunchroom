using Entities.SecurityModels;

namespace Contracts;

public interface ISecurityRepository
{
    Task<EmailValidation> GetEmailValidation(string email);
    void CreateEmailValidation(EmailValidation emailValidation);
    void DeleteEmailValidation(EmailValidation emailValidation);
}