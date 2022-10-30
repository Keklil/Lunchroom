using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) 
            : base(repositoryContext)
        {

        }

        public async Task<User> GetUserAsync(Guid userId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(userId), trackChanges)
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

        public async Task<List<User>> GetListUsersByIds(List<Guid> ids)
        {
            return await _repositoryContext.Users.Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await FindByCondition(x => x.Email == email, true)
                .SingleOrDefaultAsync();
        }
    }
}
