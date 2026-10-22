namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения позиции заказа — с вложенными данными заказа и товара.
    /// </summary>
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        /// <summary>Товар позиции.</summary>
        public ProductDTO? Product { get; set; }
    }
}
