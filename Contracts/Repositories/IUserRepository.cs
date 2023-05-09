using Domain.Infrastructure;
using Domain.Models;

namespace Contracts.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> GetUserAsync(Guid userId, bool trackChanges = true);
    Task<List<User>> GetListUsersByIds(List<Guid> ids);
    void CreateUser(User user);
    void DeleteUser(User user);
    void UpdateUser(User user);
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<Guid>> GetUserGroupIdsAsync(Guid userId);
    Task<IReadOnlyCollection<Group>> GetUserGroupAsync(Guid userId);
    void AddUserDeviceInfo(UserDeviceInfo userDeviceInfo);
    Task<UserDeviceInfo?> GetUserDeviceInfoAsync(Guid userId);
    Task<IReadOnlyList<UserDeviceInfo>> GetUsersDevicesInfoBySelectedKitchenInGroups(Guid kitchenId, bool trackChanges = true);
    Task<IReadOnlyList<Guid>> GetUserKitchenIdsAsync(Guid userId);
}