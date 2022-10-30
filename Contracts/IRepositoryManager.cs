namespace Contracts
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IMenuRepository Menu { get; }
        IOrderRepository Order { get; }
        ISecurityRepository Security { get; }
        Task SaveAsync();
    }
}
