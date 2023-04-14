using Domain.Models;

namespace Contracts.Repositories;

public interface IKitchenRepository
{
    Task<Kitchen> GetKitchenAsync(Guid kitchenId, bool trackChanges = true);
    Task<KitchenSettings?> GetKitchenSettingsAsync(Guid kitchenId, bool trackChanges = true);
    void CreateKitchen(Kitchen kitchen);
    void UpdateKitchen(Kitchen kitchen);
    void DeleteKitchen(Kitchen kitchen);
}