using System.Collections.Generic;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения категории — с вложенными подкатегориями и товарами (один-ко-многим).
    /// </summary>
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }

        /// <summary>Родительская категория (self-reference, многие-к-одному).</summary>
        public CategoryDTO? ParentCategory { get; set; }

        /// <summary>Дочерние категории (один-ко-многим).</summary>
        public List<CategoryDTO> Subcategories { get; set; } = new();

        /// <summary>Товары категории (один-ко-многим).</summary>
        public List<ProductDTO> Products { get; set; } = new();
    }
}
