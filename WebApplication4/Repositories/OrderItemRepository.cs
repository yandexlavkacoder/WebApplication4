using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<OrderItem> Query() =>
        Context.OrderItems
            .Include(i => i.Product).ThenInclude(p => p.Category);
}
