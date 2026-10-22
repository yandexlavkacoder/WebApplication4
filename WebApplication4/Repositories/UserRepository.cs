using System;
using System.Linq.Expressions;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<User?> GetByIdAsync(int id, params Expression<Func<User, object>>[] includes)
        {
            if (includes.Length == 0)
                return base.GetByIdAsync(id, u => u.Role);
            return base.GetByIdAsync(id, includes);
        }
    }
}
