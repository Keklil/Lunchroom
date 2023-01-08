using Contracts;
using Contracts.Repositories;
using Domain.DataTransferObjects;
using Domain.DataTransferObjects.User;
using Domain.Models;
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

        public async Task<List<Order>> GetOrdersByDateAsync(DateTime date, Guid groupId)
        {
            return await FindByCondition(x => x.OrderDate.Date == date && x.GroupId == groupId, false)
                .Include(x => x.Options)
                .ToListAsync();
        }

        public async Task<List<OrdersForUser>> GetOrdersByUserAsync(Guid userId, Guid groupId)
        {
            var list = await _repositoryContext.Orders
                .Where(x => x.CustomerId.Equals(userId) && x.GroupId.Equals(groupId))
                .Select(x => new OrdersForUser() { Date = x.OrderDate, Id = x.Id })
                .OrderByDescending(x => x.Date)
                .AsNoTracking()
                .ToListAsync();
            return list;
        }
        
        public async Task<List<OrdersForUser>> GetTodayOrdersByUserAsync(Guid userId, Guid groupId)
        {
            var list = await _repositoryContext.Orders
                .Where(x => x.CustomerId.Equals(userId) && x.OrderDate.Date == DateTime.UtcNow.Date)
                .Where(x => x.GroupId.Equals(groupId))
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
