namespace WebApplication4.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}