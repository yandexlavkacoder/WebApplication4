using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ShopDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken ct) =>
        await Context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName, ct);
}
