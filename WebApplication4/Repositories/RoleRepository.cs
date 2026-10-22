using WebApplication4;

namespace WebApplication4.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ShopDbContext context) : base(context)
        {
        }
    }
}
