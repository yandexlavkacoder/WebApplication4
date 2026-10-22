using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<User> Query() =>
        Context.Users
            .Include(u => u.Role);
}
