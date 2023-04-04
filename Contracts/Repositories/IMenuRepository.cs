using Domain.Models;

namespace Contracts.Repositories;

public interface IMenuRepository
{
    Task<Menu?> GetMenuAsync(Guid menuId, bool trackChanges = true);
    Task<Menu?> GetMenuByDateAsync(DateTime date, Guid groupId);
    Task<List<Menu>> GetMenuByGroup(Guid groupId);
    void CreateMenu(Menu menu);
    void UpdateMenu(Menu menu);
}