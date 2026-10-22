using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    public class OrderItemService
        : Service<OrderItem, OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>,
          IOrderItemService
    {
        public OrderItemService(IOrderItemRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
