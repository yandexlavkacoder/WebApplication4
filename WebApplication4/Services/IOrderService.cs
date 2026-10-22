using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface IOrderService : IService<OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>
{
}
