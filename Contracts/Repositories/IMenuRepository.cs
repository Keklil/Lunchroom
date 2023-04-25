using Domain.Models;

namespace Contracts.Repositories;

public interface IMenuRepository
{
    Task<Menu> GetMenuAsync(Guid menuId, bool trackChanges = true);
    Task<Menu> GetMenuByDateAsync(DateTime date, Guid kitchenId);
    Task<List<Menu>> GetMenuByGroup(Guid kitchenId);
    void CreateMenu(Menu menu);
    void UpdateMenu(Menu menu);
}