using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class StockReservationRepository : Repository<StockReservation>, IStockReservationRepository
{
    public StockReservationRepository(ShopDbContext context) : base(context)
    {
    }

    protected override IQueryable<StockReservation> Query() =>
        Context.StockReservations
            .Include(r => r.User).ThenInclude(u => u.Role)
            .Include(r => r.Product).ThenInclude(p => p.Category);

    public async Task<StockReservation?> GetActiveAsync(int id, CancellationToken ct) =>
        await Query().FirstOrDefaultAsync(r => r.StockReservationId == id && r.ReleasedAt == null, ct);

    public async Task<bool> HasActiveAsync(int userId, int productId, CancellationToken ct) =>
        await Context.StockReservations.AnyAsync(
            r => r.UserId == userId && r.ProductId == productId && r.ReleasedAt == null, ct);
}
