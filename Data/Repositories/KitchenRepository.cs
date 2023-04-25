using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Data.Repositories;

internal class KitchenRepository : RepositoryBase<Kitchen>, IKitchenRepository
{
    public async Task<Kitchen> GetKitchenAsync(Guid kitchenId, bool trackChanges = true)
    {
        var kitchen = await RepositoryContext.Kitchens.Where(x => x.Id.Equals(kitchenId))
            .Include(x => x.Settings)
            .SingleOrDefaultAsync();
        
        if (kitchen is null)
            throw new NotFoundException($"Столовая с id {kitchenId} не найдена.");

        return kitchen;
    }

    public async Task<KitchenSettings?> GetKitchenSettingsAsync(Guid kitchenId, bool trackChanges = true)
    {
        var kitchenSettings = await RepositoryContext.KitchenSettings.Where(x => x.KitchenId.Equals(kitchenId))
            .SingleOrDefaultAsync();
        
        if (kitchenSettings is null)
            throw new NotFoundException($"Столовая с id {kitchenId} не найдена.");
        
        return kitchenSettings;
    }

    public void CreateKitchen(Kitchen kitchen)
    {
        Create(kitchen);
    }

    public void UpdateKitchen(Kitchen kitchen)
    {
        Update(kitchen);
    }

    public void DeleteKitchen(Kitchen kitchen)
    {
        Delete(kitchen);
    }

    public async Task<List<Kitchen>> GetKitchensByLocationAsync(Point location, bool trackChanges = true)
    {
        var kitchens = await RepositoryContext.Kitchens
            .Include(x => x.Settings)
            .Where(x => x.Settings != null)
            .Where(x => x.Settings.ShippingAreas.Any(s => s.Polygon.Contains(location)))
            .ToListAsync();
        
        return kitchens;
    }

    public KitchenRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
}