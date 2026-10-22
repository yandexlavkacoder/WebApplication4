using System.Threading.Tasks;
using WebApplication4.DTO;

namespace WebApplication4.Services
{
    /// <summary>
    /// Сервис чтения записей о резервах (фактах бизнес-операций).
    /// </summary>
    public interface IStockReservationService
    {
        Task<PagedResult<StockReservationDTO>> GetPagedAsync(int page, int pageSize);

        Task<StockReservationDTO?> GetByIdAsync(int id);
    }
}
