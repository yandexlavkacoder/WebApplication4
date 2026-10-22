using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<Product?> GetByIdAsync(int id, params Expression<Func<Product, object>>[] includes)
        {
            // По умолчанию товар читается вместе со своей категорией.
            if (includes.Length == 0)
                return base.GetByIdAsync(id, p => p.Category);
            return base.GetByIdAsync(id, includes);
        }
    }
}
