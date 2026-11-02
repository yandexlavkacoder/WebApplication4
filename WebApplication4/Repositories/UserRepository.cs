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

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct) =>
        await Query().FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task<bool> UsernameOrEmailTakenAsync(string username, string email, CancellationToken ct) =>
        await Context.Users.AnyAsync(u => u.Username == username || u.Email == email, ct);
}
