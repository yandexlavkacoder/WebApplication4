using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class OrderItemService : Service<OrderItem, OrderItemDTO, CreateOrderItemDTO, UpdateOrderItemDTO, PatchOrderItemDTO>, IOrderItemService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderItemService(IOrderItemRepository repository, IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    protected override async Task ValidateAsync(OrderItem item, CancellationToken ct)
    {
        if (item.Quantity <= 0)
            throw new InvalidOperationException("Количество должно быть больше нуля.");
        if (item.UnitPrice < 0)
            throw new InvalidOperationException("Цена не может быть отрицательной.");
        if (!await _orderRepository.ExistsAsync(item.OrderId, ct))
            throw new InvalidOperationException($"Заказ с Id {item.OrderId} не найден.");
        if (!await _productRepository.ExistsAsync(item.ProductId, ct))
            throw new InvalidOperationException($"Товар с Id {item.ProductId} не найден.");
    }
}
