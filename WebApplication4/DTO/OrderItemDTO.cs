namespace WebApplication4.DTO;

public class OrderItemDTO
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductDTO Product { get; set; } = null!;
}
