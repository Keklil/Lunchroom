using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

internal class GroupRepository : RepositoryBase<Group>, IGroupRepository
{
    public async Task<Group?> GetGroupAsync(Guid groupId, bool trackChanges)
    {
        var group = await _repositoryContext.Groups.Where(x => x.Id.Equals(groupId))
            .Include(x => x.Members)
            .Include(x => x.Admin)
            .Include(x => x.PaymentInfo)
            .Include(x => x.Settings)
            .SingleOrDefaultAsync();

        return group;
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

    public GroupRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
}