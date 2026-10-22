using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface IOrderItemService : IService<OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>
{
}
