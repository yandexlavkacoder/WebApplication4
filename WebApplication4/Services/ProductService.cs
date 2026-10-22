using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class ProductService : Service<Product, ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>, IProductService
{
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository repository, ICategoryRepository categoryRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _categoryRepository = categoryRepository;
    }

    protected override async Task ValidateAsync(Product product, CancellationToken ct)
    {
        if (product.Price < 0)
            throw new InvalidOperationException("Цена не может быть отрицательной.");
        if (product.StockQuantity < 0)
            throw new InvalidOperationException("Количество на складе не может быть отрицательным.");
        if (!await _categoryRepository.ExistsAsync(product.CategoryId, ct))
            throw new InvalidOperationException($"Категория с Id {product.CategoryId} не найдена.");
    }
}
