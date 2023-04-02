using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

internal class UserRepository : RepositoryBase<User>, IUserRepository
{
    public async Task<User?> GetUserAsync(Guid userId, bool trackChanges)
    {
        return await FindByCondition(x => x.Id.Equals(userId), trackChanges)
            .Include(x => x.Groups)
            .SingleOrDefaultAsync();
    }

    public void CreateUser(User user)
    {
        Create(user);
    }

    public void UpdateUser(User user)
    {
        Update(user);
    }

    public void DeleteUser(User user)
    {
        Delete(user);
    }

    public async Task<List<User>?> GetListUsersByIds(List<Guid> ids)
    {
        return await _repositoryContext.Users.Where(x => ids.Contains(x.Id))
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await FindByCondition(x => x.Email == email, true)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Guid>> GetUserGroupIdsAsync(Guid userId)
    {
        var user = await _repositoryContext.Users
            .Include(x => x.Groups)
            .Where(x => x.Id.Equals(userId))
            .Select(x => x.Groups)
            .SingleOrDefaultAsync();

        var userGroups = user?.Select(x => x.Id).ToList();
        return userGroups;
    }

    public UserRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
}