namespace WebApplication4.DTO;

public class OrderDTO
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public string ShippingAddress { get; set; } = "";
    public string? PaymentMethod { get; set; }
    public UserDTO User { get; set; } = null!;
    public List<OrderItemDTO> OrderItems { get; set; } = new();
}
