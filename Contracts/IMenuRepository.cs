using Entities.Models;

namespace Contracts
{
    public interface IMenuRepository
    {
        Task<Menu> GetMenuAsync(Guid menuId, bool trackChanges);
        Task<Menu> GetMenuByDateAsync(DateTime date);
        Task<List<Menu>> GetMenus();
        void CreateMenu(Menu menu);
        void UpdateMenu(Menu menu);
    }
}
