using Domain.Models;

namespace Contracts.Repositories;

public interface IGroupRepository
{
    public Task<Group?> GetGroupAsync(Guid groupId, bool trackChanges = true);
    public void CreateGroup(Group group);
    public void UpdateGroup(Group group);
    public void DeleteOrder(Group group);
}