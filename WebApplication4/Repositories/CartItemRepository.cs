using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class CartItemRepository : Repository<CartItem>, ICartItemRepository
{
    public CartItemRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<CartItem> Query() =>
        Context.CartItems
            .Include(i => i.Product).ThenInclude(p => p.Category);
}
