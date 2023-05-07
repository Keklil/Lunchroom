using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class MenuRepository : RepositoryBase<Menu>, IMenuRepository
{
    public async Task<Menu> GetMenuAsync(Guid menuId, bool trackChanges = true)
    {
        var menu = await FindByCondition(x => x.Id.Equals(menuId), trackChanges)
            .Include(x => x.LunchSets)
            .Include(x => x.Options)
            .SingleOrDefaultAsync();
        
        if (menu is null)
            throw new NotFoundException("Меню с id: {MenuId} не найдено.", menuId);
        
        return menu;
    }

    public async Task<Menu> GetMenuByDateAsync(DateTime date, Guid kitchenId)
    {
        var dateSearch = date.ToUniversalTime().Date;
        var menu = await FindByCondition(x => x.CreatedAt.Date.Equals(dateSearch) && x.KitchenId.Equals(kitchenId), false)
            .Include(x => x.LunchSets)
            .Include(x => x.Options)
            .FirstOrDefaultAsync();

        if (menu is null)
            throw new NotFoundException("Меню не найдено для даты: {SearchDate}", date);
        
        return menu;
    }

    public async Task<List<Menu>> GetMenuByGroup(Guid kitchenId)
    {
        var menus = await RepositoryContext.Menu
            .Where(x => x.KitchenId.Equals(kitchenId))
            .OrderByDescending(x => x.CreatedAt)
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