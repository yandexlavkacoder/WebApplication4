using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<OrderItem?> GetByIdAsync(int id, params Expression<Func<OrderItem, object>>[] includes)
        {
            if (includes.Length == 0)
                return base.GetByIdAsync(id, oi => oi.Order, oi => oi.Product);
            return base.GetByIdAsync(id, includes);
        }
    }
}
