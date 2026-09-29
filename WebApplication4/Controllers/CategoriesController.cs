using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public CategoriesController(ShopDbContext context)
        {
            _context = context;
        }

        private static CategoryDTO ToDto(Category c) => new CategoryDTO
        {
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName,
            Description = c.Description,
            ParentCategoryId = c.ParentCategoryId
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.Categories
                .OrderBy(c => c.CategoryId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var c = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (c is null) return NotFound();
            return Ok(ToDto(c));
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDTO dto)
        {
            var c = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId
            };

            _context.Categories.Add(c);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = c.CategoryId }, ToDto(c));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCategoryDTO dto)
        {
            var c = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (c is null) return NotFound();

            c.CategoryName = dto.CategoryName;
            c.Description = dto.Description;
            c.ParentCategoryId = dto.ParentCategoryId;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCategoryDTO dto)
        {
            var c = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (c is null) return NotFound();

            if (dto.CategoryName is not null) c.CategoryName = dto.CategoryName;
            if (dto.Description is not null) c.Description = dto.Description;
            if (dto.ParentCategoryId is not null) c.ParentCategoryId = dto.ParentCategoryId.Value;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var c = _context.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (c is null) return NotFound();

            _context.Categories.Remove(c);
            _context.SaveChanges();
            return NoContent();
        }
    }
}