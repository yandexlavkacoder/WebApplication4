using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class CategoryService : Service<Category, CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>, ICategoryService
{
    public CategoryService(ICategoryRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }

    protected override async Task ValidateAsync(Category category, CancellationToken ct)
    {
        if (category.ParentCategoryId is null) return;
        if (category.ParentCategoryId == category.CategoryId)
            throw new InvalidOperationException("Категория не может быть родителем самой себя.");
        if (!await Repository.ExistsAsync(category.ParentCategoryId.Value, ct))
            throw new InvalidOperationException($"Родительская категория с Id {category.ParentCategoryId} не найдена.");
    }
}
