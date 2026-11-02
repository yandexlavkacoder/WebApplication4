using WebApplication4.Authorization;

namespace WebApplication4.DTO;

public class StockReservationDTO : IOwnedResource
{
    public int StockReservationId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public UserDTO User { get; set; } = null!;
    public ProductDTO Product { get; set; } = null!;

    int IOwnedResource.OwnerUserId => User.UserId;
}
