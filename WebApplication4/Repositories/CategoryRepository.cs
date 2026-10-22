using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<Category> Query() =>
        Context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.Products);
}
