namespace WebApplication4.DTO
{
    public class CreateCategoryDTO
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}