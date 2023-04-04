using Contracts.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class MenuRepository : RepositoryBase<Menu>, IMenuRepository
{
    public async Task<Menu?> GetMenuAsync(Guid menuId, bool trackChanges = true)
    {
        return await FindByCondition(x => x.Id.Equals(menuId), trackChanges)
            .Include(x => x.LunchSets)
            .Include(x => x.Options)
            .SingleOrDefaultAsync();
    }

    public async Task<Menu?> GetMenuByDateAsync(DateTime date, Guid groupId)
    {
        var dateSearch = date.ToUniversalTime().Date;
        return await FindByCondition(x => x.Date.Date.Equals(dateSearch) && x.GroupId.Equals(groupId), false)
            .Include(x => x.LunchSets)
            .Include(x => x.Options)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Menu>> GetMenuByGroup(Guid groupId)
    {
        var menus = await RepositoryContext.Menu
            .Where(x => x.GroupId.Equals(groupId))
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

    public MenuRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
}