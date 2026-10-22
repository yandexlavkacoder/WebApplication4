namespace WebApplication4.DTO;

public class CartItemDTO
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public DateTime? AddedAt { get; set; }
    public ProductDTO Product { get; set; } = null!;
}
