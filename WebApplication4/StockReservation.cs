namespace WebApplication4;

public class StockReservation
{
    public int StockReservationId { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
