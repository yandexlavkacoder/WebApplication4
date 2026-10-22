using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface IStockReservationService
{
    Task<(List<StockReservationDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<StockReservationDTO?> GetByIdAsync(int id, CancellationToken ct);
    Task<StockReservationDTO> ReserveAsync(CreateStockReservationDTO dto, CancellationToken ct);
    Task<StockReservationDTO?> ReleaseAsync(int id, CancellationToken ct);
}
