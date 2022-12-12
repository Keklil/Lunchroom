using Domain.SecurityModels;

namespace Contracts.Repositories;

public interface ISecurityRepository
{
    Task<EmailValidation> GetEmailValidation(string email);
    void CreateEmailValidation(EmailValidation emailValidation);
    void DeleteEmailValidation(EmailValidation emailValidation);
}