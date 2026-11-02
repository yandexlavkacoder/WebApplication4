using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Authorization;
using WebApplication4.DTO;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockReservationsController : ControllerBase
{
    private readonly IStockReservationService _service;
    private readonly IAuthorizationService _authorizationService;

    public StockReservationsController(IStockReservationService service, IAuthorizationService authorizationService)
    {
        _service = service;
        _authorizationService = authorizationService;
    }

    [Authorize(Policy = Policies.CanManageCatalog)]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _service.GetAllAsync(page, pageSize, cancellationToken);
        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var dto = await _service.GetByIdAsync(id, cancellationToken);
        if (dto is null) return NotFound();

        if (!await IsOwnerOrAdminAsync(dto)) return Forbid();
        return Ok(dto);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Reserve(CreateStockReservationDTO dto, CancellationToken cancellationToken)
    {
        if (!await IsOwnerOrAdminAsync(dto)) return Forbid();

        try
        {
            var reservation = await _service.ReserveAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = reservation.StockReservationId }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("{id}/release")]
    public async Task<IActionResult> Release(int id, CancellationToken cancellationToken)
    {
        var existing = await _service.GetByIdAsync(id, cancellationToken);
        if (existing is null)
            return NotFound(new { error = $"Резерв с Id {id} не найден." });

        if (!await IsOwnerOrAdminAsync(existing)) return Forbid();

        var reservation = await _service.ReleaseAsync(id, cancellationToken);
        if (reservation is null)
            return NotFound(new { error = $"Активный резерв с Id {id} не найден." });
        return Ok(reservation);
    }

    private async Task<bool> IsOwnerOrAdminAsync(IOwnedResource resource) =>
        (await _authorizationService.AuthorizeAsync(User, resource, Policies.OwnerOrAdmin)).Succeeded;
}
