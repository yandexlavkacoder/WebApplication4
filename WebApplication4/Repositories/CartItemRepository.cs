using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<CartItem?> GetByIdAsync(int id, params Expression<Func<CartItem, object>>[] includes)
        {
            if (includes.Length == 0)
                return base.GetByIdAsync(id, ci => ci.Cart, ci => ci.Product);
            return base.GetByIdAsync(id, includes);
        }
    }
}
