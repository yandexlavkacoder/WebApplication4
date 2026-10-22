using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public class StockReservationRepository : Repository<StockReservation>, IStockReservationRepository
    {
        public StockReservationRepository(ShopDbContext context) : base(context)
        {
        }

        public override Task<StockReservation?> GetByIdAsync(int id, params Expression<Func<StockReservation, object>>[] includes)
        {
            if (includes.Length == 0)
                return base.GetByIdAsync(id, r => r.User, r => r.Product);
            return base.GetByIdAsync(id, includes);
        }

        public async Task<StockReservation?> GetActiveByUserAndProductAsync(int userId, int productId)
        {
            return await Set
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.UserId == userId
                    && r.ProductId == productId
                    && r.ReleasedAt == null);
        }
    }
}
