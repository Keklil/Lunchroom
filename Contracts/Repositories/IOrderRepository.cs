using Domain.DataTransferObjects;
using Domain.DataTransferObjects.User;
using Domain.Models;

namespace Contracts.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderAsync(Guid orderId, bool trackChanges);
        Task<List<Order>> GetOrdersByDateAsync(DateTime date, Guid groupId);
        Task<List<OrdersForUser>> GetOrdersByUserAsync(Guid userId, Guid groupId);
        Task<List<OrdersForUser>> GetTodayOrdersByUserAsync(Guid userId, Guid groupId);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
