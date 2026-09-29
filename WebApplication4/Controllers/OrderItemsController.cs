using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public OrderItemsController(ShopDbContext context)
        {
            _context = context;
        }

        private static OrderItemDTO ToDto(OrderItem i) => new OrderItemDTO
        {
            OrderItemId = i.OrderItemId,
            OrderId = i.OrderId,
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.OrderItems
                .OrderBy(i => i.OrderItemId)
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
            var i = _context.OrderItems.FirstOrDefault(x => x.OrderItemId == id);
            if (i is null) return NotFound();
            return Ok(ToDto(i));
        }

        [HttpPost]
        public IActionResult Create(CreateOrderItemDTO dto)
        {
            var i = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            _context.OrderItems.Add(i);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = i.OrderItemId }, ToDto(i));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateOrderItemDTO dto)
        {
            var i = _context.OrderItems.FirstOrDefault(x => x.OrderItemId == id);
            if (i is null) return NotFound();

            i.OrderId = dto.OrderId;
            i.ProductId = dto.ProductId;
            i.Quantity = dto.Quantity;
            i.UnitPrice = dto.UnitPrice;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchOrderItemDTO dto)
        {
            var i = _context.OrderItems.FirstOrDefault(x => x.OrderItemId == id);
            if (i is null) return NotFound();

            if (dto.OrderId is not null) i.OrderId = dto.OrderId.Value;
            if (dto.ProductId is not null) i.ProductId = dto.ProductId.Value;
            if (dto.Quantity is not null) i.Quantity = dto.Quantity.Value;
            if (dto.UnitPrice is not null) i.UnitPrice = dto.UnitPrice.Value;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var i = _context.OrderItems.FirstOrDefault(x => x.OrderItemId == id);
            if (i is null) return NotFound();

            _context.OrderItems.Remove(i);
            _context.SaveChanges();
            return NoContent();
        }
    }
}