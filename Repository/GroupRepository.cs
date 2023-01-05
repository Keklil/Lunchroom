using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task<Group?> GetGroupAsync(Guid groupId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(groupId), trackChanges)
                .Include(x => x.Members)
                .SingleOrDefaultAsync();
        }
        public void CreateGroup(Group group)
        {
            Create(group);
        }

        public void UpdateGroup(Group group)
        {
            Update(group);
        }

        public void DeleteOrder(Group group)
        {
            Delete(group);
        }
    }
}
