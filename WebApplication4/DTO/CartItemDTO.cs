using System;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения элемента корзины — с вложенным товаром.
    /// </summary>
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime? AddedAt { get; set; }

        /// <summary>Товар в корзине.</summary>
        public ProductDTO? Product { get; set; }
    }
}
