using System;

namespace WebApplication4.DTO
{
    public class UpdateCartItemDTO
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? AddedAt { get; set; }
    }
}