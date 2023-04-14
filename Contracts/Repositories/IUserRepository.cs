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
}