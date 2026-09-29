using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public CartsController(ShopDbContext context)
        {
            _context = context;
        }

        private static CartDTO ToDto(Cart c) => new CartDTO
        {
            CartId = c.CartId,
            UserId = c.UserId,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.Carts
                .OrderBy(c => c.CartId)
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
            var c = _context.Carts.FirstOrDefault(x => x.CartId == id);
            if (c is null) return NotFound();
            return Ok(ToDto(c));
        }

        [HttpPost]
        public IActionResult Create(CreateCartDTO dto)
        {
            var c = new Cart
            {
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(c);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = c.CartId }, ToDto(c));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCartDTO dto)
        {
            var c = _context.Carts.FirstOrDefault(x => x.CartId == id);
            if (c is null) return NotFound();

            c.UserId = dto.UserId;
            c.CreatedAt = dto.CreatedAt;
            c.UpdatedAt = dto.UpdatedAt;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCartDTO dto)
        {
            var c = _context.Carts.FirstOrDefault(x => x.CartId == id);
            if (c is null) return NotFound();

            if (dto.UserId is not null) c.UserId = dto.UserId.Value;
            if (dto.CreatedAt is not null) c.CreatedAt = dto.CreatedAt.Value;
            if (dto.UpdatedAt is not null) c.UpdatedAt = dto.UpdatedAt.Value;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var c = _context.Carts.FirstOrDefault(x => x.CartId == id);
            if (c is null) return NotFound();

            _context.Carts.Remove(c);
            _context.SaveChanges();
            return NoContent();
        }
    }
}