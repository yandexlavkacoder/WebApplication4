using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ShopDbContext context) : base(context)
        {
        }

        public override async Task<Category?> GetByIdAsync(int id, params Expression<Func<Category, object>>[] includes)
        {
            if (includes.Length == 0)
                return await base.GetByIdAsync(id, c => c.ParentCategory, c => c.Products);
            return await base.GetByIdAsync(id, includes);
        }
    }
}
