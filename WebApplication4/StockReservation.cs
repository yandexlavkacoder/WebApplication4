using System;

namespace WebApplication4;

/// <summary>
/// Сущность-факт бизнес-операции: кто (User), что (Product и количество), когда (ReservedAt)
/// зарезервировал товар со склада, а также когда резерв был отменён (ReleasedAt).
/// Active = ReservedAt != null && ReleasedAt == null.
/// </summary>
public partial class StockReservation
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
