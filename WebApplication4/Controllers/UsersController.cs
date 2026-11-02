using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Authorization;
using WebApplication4.DTO;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IAuthorizationService _authorizationService;

    public UsersController(IUserService service, IAuthorizationService authorizationService)
    {
        _service = service;
        _authorizationService = authorizationService;
    }

    [Authorize(Policy = Policies.AdminOnly)]
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

    [Authorize(Policy = Policies.AdminOnly)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _service.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateUserDTO dto, CancellationToken cancellationToken)
    {
        var existing = await _service.GetByIdAsync(id, cancellationToken);
        if (existing is null) return NotFound();

        if (!await IsOwnerOrAdminAsync(existing)) return Forbid();
        if (!User.IsInRole(Roles.Admin) &&
            (dto.RoleId != existing.Role.RoleId || dto.IsActive != existing.IsActive))
            return Forbid();

        try
        {
            var updated = await _service.UpdateAsync(id, dto, cancellationToken);
            if (updated is null) return NotFound();
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchUserDTO dto, CancellationToken cancellationToken)
    {
        var existing = await _service.GetByIdAsync(id, cancellationToken);
        if (existing is null) return NotFound();

        if (!await IsOwnerOrAdminAsync(existing)) return Forbid();
        if (!User.IsInRole(Roles.Admin) && (dto.RoleId is not null || dto.IsActive is not null))
            return Forbid();

        try
        {
            var updated = await _service.PatchAsync(id, dto, cancellationToken);
            if (updated is null) return NotFound();
            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [Authorize(Policy = Policies.AdminOnly)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id, cancellationToken);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private async Task<bool> IsOwnerOrAdminAsync(UserDTO user) =>
        (await _authorizationService.AuthorizeAsync(User, user, Policies.OwnerOrAdmin)).Succeeded;
}
