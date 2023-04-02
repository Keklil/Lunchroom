using Contracts.Repositories;
using Domain.SecurityModels;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class SecurityRepository : RepositoryBase<EmailValidation>, ISecurityRepository
{
    public async Task<EmailValidation> GetEmailValidation(string email)
    {
        return await FindByCondition(x => x.Email.Equals(email), true)
            .SingleOrDefaultAsync();
    }

    public void CreateEmailValidation(EmailValidation emailValidation)
    {
        Create(emailValidation);
    }

    public void DeleteEmailValidation(EmailValidation emailValidation)
    {
        Delete(emailValidation);
    }

    public SecurityRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
}