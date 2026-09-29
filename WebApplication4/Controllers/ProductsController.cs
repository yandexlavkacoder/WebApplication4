using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public ProductsController(ShopDbContext context)
        {
            _context = context;
        }

        private static ProductDTO ToDto(Product p) => new ProductDTO
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CategoryId = p.CategoryId,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            ImagePath = p.ImagePath
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.Products
                .OrderBy(p => p.ProductId)
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
            var p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (p is null) return NotFound();
            return Ok(ToDto(p));
        }

        [HttpPost]
        public IActionResult Create(CreateProductDTO dto)
        {
            var p = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                ImagePath = dto.ImagePath
            };

            _context.Products.Add(p);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = p.ProductId }, ToDto(p));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProductDTO dto)
        {
            var p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (p is null) return NotFound();

            p.ProductName = dto.ProductName;
            p.Description = dto.Description;
            p.Price = dto.Price;
            p.StockQuantity = dto.StockQuantity;
            p.CategoryId = dto.CategoryId;
            p.IsActive = dto.IsActive;
            p.CreatedAt = dto.CreatedAt;
            p.ImagePath = dto.ImagePath;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchProductDTO dto)
        {
            var p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (p is null) return NotFound();

            if (dto.ProductName is not null) p.ProductName = dto.ProductName;
            if (dto.Description is not null) p.Description = dto.Description;
            if (dto.Price is not null) p.Price = dto.Price.Value;
            if (dto.StockQuantity is not null) p.StockQuantity = dto.StockQuantity.Value;
            if (dto.CategoryId is not null) p.CategoryId = dto.CategoryId.Value;
            if (dto.IsActive is not null) p.IsActive = dto.IsActive.Value;
            if (dto.CreatedAt is not null) p.CreatedAt = dto.CreatedAt.Value;
            if (dto.ImagePath is not null) p.ImagePath = dto.ImagePath;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var p = _context.Products.FirstOrDefault(x => x.ProductId == id);
            if (p is null) return NotFound();

            _context.Products.Remove(p);
            _context.SaveChanges();
            return NoContent();
        }
    }
}