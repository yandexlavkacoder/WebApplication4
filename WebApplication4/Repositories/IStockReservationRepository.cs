namespace WebApplication4.Repositories;

public interface IStockReservationRepository : IRepository<StockReservation>
{
    Task<StockReservation?> GetActiveAsync(int id, CancellationToken ct);
    Task<bool> HasActiveAsync(int userId, int productId, CancellationToken ct);
}
