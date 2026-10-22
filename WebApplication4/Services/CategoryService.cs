using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    public class CategoryService
        : Service<Category, CategoryDTO, CreateCategoryDTO, UpdateCategoryDTO, PatchCategoryDTO>,
          ICategoryService
    {
        public CategoryService(ICategoryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
