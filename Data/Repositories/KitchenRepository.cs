using Contracts.Repositories;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class KitchenRepository : RepositoryBase<Kitchen>, IKitchenRepository
{
    public async Task<Kitchen> GetKitchenAsync(Guid kitchenId, bool trackChanges = true)
    {
        var kitchen = await RepositoryContext.Kitchens.Where(x => x.Id.Equals(kitchenId))
            .Include(x => x.Settings)
            .SingleOrDefaultAsync();
        
        if (kitchen is null)
            throw new NotFoundException("Запрашиваемая группа не найдена");

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

    public KitchenRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
}