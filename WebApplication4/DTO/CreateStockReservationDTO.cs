using WebApplication4.Authorization;

namespace WebApplication4.DTO;

public class CreateStockReservationDTO : IOwnedResource
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    int IOwnedResource.OwnerUserId => UserId;
}
