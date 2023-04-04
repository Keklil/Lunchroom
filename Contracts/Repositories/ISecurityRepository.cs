using Domain.SecurityModels;

namespace Contracts.Repositories;

public interface ISecurityRepository
{
    Task<EmailValidation?> GetEmailValidation(string email, bool trackChanges = true);
    void CreateEmailValidation(EmailValidation emailValidation);
    void DeleteEmailValidation(EmailValidation emailValidation);
}