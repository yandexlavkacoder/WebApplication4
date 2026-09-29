namespace WebApplication4.DTO
{
    public class UpdateCategoryDTO
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}