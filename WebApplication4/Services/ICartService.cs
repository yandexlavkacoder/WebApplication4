using System.Threading.Tasks;
using WebApplication4.DTO;

namespace WebApplication4.Services
{
    public interface ICartService : IService<CartDTO, CreateCartDTO, UpdateCartDTO, PatchCartDTO>
    {
    }
}
