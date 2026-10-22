using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<Cart> Query() =>
        Context.Carts
            .Include(c => c.User).ThenInclude(u => u.Role)
            .Include(c => c.CartItems).ThenInclude(i => i.Product).ThenInclude(p => p.Category);
}
