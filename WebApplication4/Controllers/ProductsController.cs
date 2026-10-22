using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    /// <summary>
    /// Тонкий контроллер: только вызов сервиса и перевод результата в HTTP-статус.
    /// Никакого DbContext и маппинга здесь нет.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET api/products?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(page, pageSize);
            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items);
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // PATCH api/products/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, PatchProductDTO dto)
        {
            var updated = await _service.PatchAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // ---------- Бизнес-операции ----------

        // POST api/products/5/reserve  { userId, productId, quantity }
        [HttpPost("{id}/reserve")]
        public async Task<IActionResult> Reserve(int id, CreateStockReservationDTO dto)
        {
            if (dto.ProductId != 0 && dto.ProductId != id)
                return BadRequest("ProductId в теле запроса должен совпадать с id в маршруте.");

            dto.ProductId = id;
            try
            {
                var reservation = await _service.ReserveStockAsync(dto);
                return CreatedAtAction(nameof(StockReservationsController.GetById),
                    new { id = reservation.StockReservationId }, reservation);
            }
            catch (BusinessException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // POST api/products/reservations/7/release
        [HttpPost("reservations/{reservationId:int}/release")]
        public async Task<IActionResult> Release(int reservationId)
        {
            try
            {
                var reservation = await _service.ReleaseStockAsync(reservationId);
                return Ok(reservation);
            }
            catch (BusinessException ex)
            {
                // Резерва нет или уже закрыт -> 404, а не 500.
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
