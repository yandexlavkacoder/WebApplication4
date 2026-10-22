namespace WebApplication4.DTO;

public class CreateOrderDTO
{
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Status { get; set; }
    public string ShippingAddress { get; set; } = "";
    public string? PaymentMethod { get; set; }
}
