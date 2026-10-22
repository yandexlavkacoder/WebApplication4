using System;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO записи: создание резерва товара. Содержит Id связанных сущностей
    /// (UserId, ProductId) — кто и что резервирует.
    /// </summary>
    public class CreateStockReservationDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
