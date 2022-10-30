using Entities.DataTransferObjects;
using Entities.Models;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderAsync(Guid orderId, bool trackChanges);
        Task<List<Order>> GetOrdersByDateAsync(DateTime date);
        Task<List<OrdersForUser>> GetOrdersByUserAsync(Guid userId);
        Task<List<OrdersForUser>> GetTodayOrdersByUserAsync(Guid userId);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
