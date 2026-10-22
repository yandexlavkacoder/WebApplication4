using System;
using System.Collections.Generic;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения корзины — с вложенными элементами (один-ко-многим).
    /// </summary>
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Владелец корзины.</summary>
        public UserDTO? User { get; set; }

        /// <summary>Элементы корзины (один-ко-многим).</summary>
        public List<CartItemDTO> CartItems { get; set; } = new();
    }
}
