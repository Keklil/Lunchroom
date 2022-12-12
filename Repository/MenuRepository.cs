using Contracts;
using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class MenuRepository : RepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task<Menu> GetMenuAsync(Guid menuId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(menuId), trackChanges)
                .Include(x => x.LunchSets)
                .Include(x => x.Options)
                .SingleOrDefaultAsync();
        }

        public async Task<Menu> GetMenuByDateAsync(DateTime date)
        {
            var dateSearch = date.ToUniversalTime().Date;
            var menu = await FindByCondition(x => x.Date.Date.Equals(dateSearch), false)
                .Include(x => x.LunchSets)
                .Include(x => x.Options)
                .FirstOrDefaultAsync();
            return menu;
        }

        public async Task<List<Menu>> GetMenus()
        {
            var menus = await _repositoryContext.Menu
                .OrderByDescending(x => x.Date)
                .ToListAsync();
            return menus;
        }
        public void CreateMenu(Menu menu)
        {
            Create(menu);
        }

        public void UpdateMenu(Menu menu)
        {
            Update(menu);
        }
    }
}
