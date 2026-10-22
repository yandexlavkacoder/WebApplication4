namespace WebApplication4.DTO;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    public List<string> Products { get; set; } = new();
}
