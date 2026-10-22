using System.Threading.Tasks;
using WebApplication4.DTO;

namespace WebApplication4.Services
{
    public interface IProductService : IService<ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>
    {
        /// <summary>
        /// Резервирование товара: уменьшает StockQuantity у Product и создаёт
        /// активную запись StockReservation (кто/что/когда). Один SaveChangesAsync.
        /// </summary>
        Task<StockReservationDTO> ReserveStockAsync(CreateStockReservationDTO dto);

        /// <summary>
        /// Обратная операция: возвращает зарезервированное количество на склад
        /// и закрывает запись резерва (ReleasedAt). Один SaveChangesAsync.
        /// </summary>
        Task<StockReservationDTO> ReleaseStockAsync(int stockReservationId);
    }
}
