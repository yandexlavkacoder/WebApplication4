using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class CartItemService : Service<CartItem, CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>, ICartItemService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartItemService(ICartItemRepository repository, ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    protected override async Task ValidateAsync(CartItem item, CancellationToken ct)
    {
        if (item.Quantity <= 0)
            throw new InvalidOperationException("Количество должно быть больше нуля.");
        if (!await _cartRepository.ExistsAsync(item.CartId, ct))
            throw new InvalidOperationException($"Корзина с Id {item.CartId} не найдена.");
        if (!await _productRepository.ExistsAsync(item.ProductId, ct))
            throw new InvalidOperationException($"Товар с Id {item.ProductId} не найден.");
    }
}
