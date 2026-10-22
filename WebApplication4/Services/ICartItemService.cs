using System.Threading.Tasks;
using WebApplication4.DTO;

namespace WebApplication4.Services
{
    public interface ICartItemService : IService<CartItemDTO, CreateCartItemDTO, UpdateCartItemDTO, PatchCartItemDTO>
    {
    }
}
