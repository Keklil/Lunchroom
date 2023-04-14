namespace Contracts.Repositories;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IMenuRepository Menu { get; }
    IOrderRepository Order { get; }
    ISecurityRepository Security { get; }
    IGroupRepository Groups { get; }
    IKitchenRepository Kitchens { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
}