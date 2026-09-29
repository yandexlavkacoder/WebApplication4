using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public CartItemsController(ShopDbContext context)
        {
            _context = context;
        }

        private static CartItemDTO ToDto(CartItem i) => new CartItemDTO
        {
            CartItemId = i.CartItemId,
            CartId = i.CartId,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            AddedAt = i.AddedAt
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.CartItems
                .OrderBy(i => i.CartItemId)
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
            var i = _context.CartItems.FirstOrDefault(x => x.CartItemId == id);
            if (i is null) return NotFound();
            return Ok(ToDto(i));
        }

        [HttpPost]
        public IActionResult Create(CreateCartItemDTO dto)
        {
            var i = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                AddedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(i);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = i.CartItemId }, ToDto(i));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCartItemDTO dto)
        {
            var i = _context.CartItems.FirstOrDefault(x => x.CartItemId == id);
            if (i is null) return NotFound();

            i.CartId = dto.CartId;
            i.ProductId = dto.ProductId;
            i.Quantity = dto.Quantity;
            i.AddedAt = dto.AddedAt;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCartItemDTO dto)
        {
            var i = _context.CartItems.FirstOrDefault(x => x.CartItemId == id);
            if (i is null) return NotFound();

            if (dto.CartId is not null) i.CartId = dto.CartId.Value;
            if (dto.ProductId is not null) i.ProductId = dto.ProductId.Value;
            if (dto.Quantity is not null) i.Quantity = dto.Quantity.Value;
            if (dto.AddedAt is not null) i.AddedAt = dto.AddedAt.Value;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var i = _context.CartItems.FirstOrDefault(x => x.CartItemId == id);
            if (i is null) return NotFound();

            _context.CartItems.Remove(i);
            _context.SaveChanges();
            return NoContent();
        }
    }
}