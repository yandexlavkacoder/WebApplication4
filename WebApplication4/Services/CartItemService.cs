using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    public class CartItemService
        : Service<CartItem, CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>,
          ICartItemService
    {
        public CartItemService(ICartItemRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
