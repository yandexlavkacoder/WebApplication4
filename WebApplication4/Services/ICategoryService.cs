using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface ICategoryService : IService<CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>
{
}
