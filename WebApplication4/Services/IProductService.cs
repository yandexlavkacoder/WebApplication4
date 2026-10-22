using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface IProductService : IService<ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>
{
}
