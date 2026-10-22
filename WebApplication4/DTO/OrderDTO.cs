using System;
using System.Collections.Generic;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения заказа — с вложенными позициями и данными пользователя (один-ко-многим).
    /// </summary>
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string? PaymentMethod { get; set; }

        /// <summary>Пользователь, оформивший заказ.</summary>
        public UserDTO? User { get; set; }

        /// <summary>Позиции заказа (один-ко-многим).</summary>
        public List<OrderItemDTO> OrderItems { get; set; } = new();
    }
}
