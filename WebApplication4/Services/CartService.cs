using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class CartService : Service<Cart, CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>, ICartService
{
    private readonly IUserRepository _userRepository;

    public CartService(ICartRepository repository, IUserRepository userRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _userRepository = userRepository;
    }

    protected override async Task ValidateAsync(Cart cart, CancellationToken ct)
    {
        if (!await _userRepository.ExistsAsync(cart.UserId, ct))
            throw new InvalidOperationException($"Пользователь с Id {cart.UserId} не найден.");
    }
}
