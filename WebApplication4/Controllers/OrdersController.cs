using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public OrdersController(ShopDbContext context)
        {
            _context = context;
        }

        private static OrderDTO ToDto(Order o) => new OrderDTO
        {
            OrderId = o.OrderId,
            UserId = o.UserId,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            ShippingAddress = o.ShippingAddress,
            PaymentMethod = o.PaymentMethod
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.Orders
                .OrderBy(o => o.OrderId)
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
            var o = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (o is null) return NotFound();
            return Ok(ToDto(o));
        }

        [HttpPost]
        public IActionResult Create(CreateOrderDTO dto)
        {
            var o = new Order
            {
                UserId = dto.UserId,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                ShippingAddress = dto.ShippingAddress,
                PaymentMethod = dto.PaymentMethod,
                OrderDate = DateTime.UtcNow
            };

            _context.Orders.Add(o);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = o.OrderId }, ToDto(o));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateOrderDTO dto)
        {
            var o = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (o is null) return NotFound();

            o.UserId = dto.UserId;
            o.OrderDate = dto.OrderDate;
            o.TotalAmount = dto.TotalAmount;
            o.Status = dto.Status;
            o.ShippingAddress = dto.ShippingAddress;
            o.PaymentMethod = dto.PaymentMethod;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchOrderDTO dto)
        {
            var o = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (o is null) return NotFound();

            if (dto.UserId is not null) o.UserId = dto.UserId.Value;
            if (dto.OrderDate is not null) o.OrderDate = dto.OrderDate.Value;
            if (dto.TotalAmount is not null) o.TotalAmount = dto.TotalAmount.Value;
            if (dto.Status is not null) o.Status = dto.Status;
            if (dto.ShippingAddress is not null) o.ShippingAddress = dto.ShippingAddress;
            if (dto.PaymentMethod is not null) o.PaymentMethod = dto.PaymentMethod;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var o = _context.Orders.FirstOrDefault(x => x.OrderId == id);
            if (o is null) return NotFound();

            _context.Orders.Remove(o);
            _context.SaveChanges();
            return NoContent();
        }
    }
}