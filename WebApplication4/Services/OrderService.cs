using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    public class OrderService
        : Service<Order, OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>,
          IOrderService
    {
        public OrderService(IOrderRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
