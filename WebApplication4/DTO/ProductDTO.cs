namespace WebApplication4.DTO;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? ImagePath { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
}
