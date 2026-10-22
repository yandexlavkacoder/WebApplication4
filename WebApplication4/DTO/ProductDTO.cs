using System;
using System.Collections.Generic;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения товара — с вложенной связанной категорией.
    /// </summary>
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ImagePath { get; set; }

        /// <summary>Вложенные данные о связанной категории (связь многие-к-одному).</summary>
        public CategoryDTO? Category { get; set; }
    }
}
