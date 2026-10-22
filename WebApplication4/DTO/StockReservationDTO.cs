using System;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения записи о факте бизнес-операции: кто (User), что (Product, Quantity),
    /// когда зарезервировал (ReservedAt) и когда резерв закрыт (ReleasedAt).
    /// </summary>
    public class StockReservationDTO
    {
        public int StockReservationId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }

        /// <summary>Пользователь, сделавший резерв (вложенные данные).</summary>
        public UserDTO? User { get; set; }

        /// <summary>Зарезервированный товар (вложенные данные).</summary>
        public ProductDTO? Product { get; set; }
    }
}
