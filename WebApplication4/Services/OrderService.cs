using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class OrderService : Service<Order, OrderDTO, CreateOrderDTO, UpdateOrderDTO, PatchOrderDTO>, IOrderService
{
    private readonly IUserRepository _userRepository;

    public OrderService(IOrderRepository repository, IUserRepository userRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _userRepository = userRepository;
    }

    protected override async Task ValidateAsync(Order order, CancellationToken ct)
    {
        if (order.TotalAmount < 0)
            throw new InvalidOperationException("Сумма заказа не может быть отрицательной.");
        if (!await _userRepository.ExistsAsync(order.UserId, ct))
            throw new InvalidOperationException($"Пользователь с Id {order.UserId} не найден.");
    }
}
