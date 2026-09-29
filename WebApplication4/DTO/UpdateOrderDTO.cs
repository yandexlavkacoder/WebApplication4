using System;

namespace WebApplication4.DTO
{
    public class UpdateOrderDTO
    {
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string? PaymentMethod { get; set; }
    }
}