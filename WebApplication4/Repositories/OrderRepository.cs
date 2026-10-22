using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<Order?> GetByIdAsync(int id, params Expression<Func<Order, object>>[] includes)
        {
            if (includes.Length == 0)
                return base.GetByIdAsync(id, o => o.User, o => o.OrderItems);
            return base.GetByIdAsync(id, includes);
        }
    }
}
