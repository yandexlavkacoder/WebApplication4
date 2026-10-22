namespace WebApplication4.DTO;

public class CartDTO
{
    public int CartId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public UserDTO User { get; set; } = null!;
    public List<CartItemDTO> CartItems { get; set; } = new();
}
