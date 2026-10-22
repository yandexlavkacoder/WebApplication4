using System.Threading.Tasks;
using WebApplication4;

namespace WebApplication4.Repositories
{
    public interface IStockReservationRepository : IRepository<StockReservation>
    {
        /// <summary>
        /// Активный резерв пользователя по товару (ReleasedAt == null).
        /// </summary>
        Task<StockReservation?> GetActiveByUserAndProductAsync(int userId, int productId);
    }
}
