using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    /// <summary>
    /// Read-only контроллер записей о резервах (фактах бизнес-операций).
    /// Резерв создаётся и отменяется через ProductsController / ProductService.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StockReservationsController : ControllerBase
    {
        private readonly IStockReservationService _service;

        public StockReservationsController(IStockReservationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(page, pageSize);
            Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
            return Ok(result.Items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }
    }
}
