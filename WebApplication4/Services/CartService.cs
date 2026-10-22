using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    public class CartService
        : Service<Cart, CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>,
          ICartService
    {
        public CartService(ICartRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
