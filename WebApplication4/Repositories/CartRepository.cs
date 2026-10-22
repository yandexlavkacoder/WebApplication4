using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<Cart?> GetByIdAsync(int id, params Expression<Func<Cart, object>>[] includes)
        {
            if (includes.Length == 0)
                return base.GetByIdAsync(id, c => c.User, c => c.CartItems);
            return base.GetByIdAsync(id, includes);
        }
    }
}
