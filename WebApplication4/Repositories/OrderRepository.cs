using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<Order> Query() =>
        Context.Orders
            .Include(o => o.User).ThenInclude(u => u.Role)
            .Include(o => o.OrderItems).ThenInclude(i => i.Product).ThenInclude(p => p.Category);
}
