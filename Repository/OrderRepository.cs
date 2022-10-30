using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public async Task<Order> GetOrderAsync(Guid orderId, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(orderId), trackChanges)
                .Include(x => x.Options)
                .SingleOrDefaultAsync();
        }
        public void CreateOrder(Order order)
        {
            Create(order);
        }

        public void UpdateOrder(Order order)
        {
            Update(order);
        }

        public async Task<List<Order>> GetOrdersByDateAsync(DateTime date)
        {
            return await FindByCondition(x => x.OrderDate.Date == date, false)
                .Include(x => x.Options)
                .ToListAsync();
        }

        public async Task<List<OrdersForUser>> GetOrdersByUserAsync(Guid userId)
        {
            var list = await _repositoryContext.Orders
                .Where(x => x.CustomerId.Equals(userId))
                .Select(x => new OrdersForUser() { Date = x.OrderDate, Id = x.Id })
                .OrderByDescending(x => x.Date)
                .AsNoTracking()
                .ToListAsync();
            return list;
        }
        
        public async Task<List<OrdersForUser>> GetTodayOrdersByUserAsync(Guid userId)
        {
            var list = await _repositoryContext.Orders
                .Where(x => x.CustomerId.Equals(userId) && x.OrderDate.Date == DateTime.UtcNow.Date)
                .Select(x => new OrdersForUser() { Date = x.OrderDate, Id = x.Id })
                .AsNoTracking()
                .ToListAsync();
            
            return list;
        }

        public void DeleteOrder(Order order)
        {
            Delete(order);
        }
    }
}
