namespace WebApplication4.DTO
{
    public class PatchOrderItemDTO
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}