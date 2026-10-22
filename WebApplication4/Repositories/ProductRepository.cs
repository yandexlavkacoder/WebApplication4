using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<Product> Query() =>
        Context.Products
            .Include(p => p.Category);
}
