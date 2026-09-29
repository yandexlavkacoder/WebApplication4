using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public RolesController(ShopDbContext context)
        {
            _context = context;
        }

        private static RoleDTO ToDto(Role r) => new RoleDTO
        {
            RoleId = r.RoleId,
            RoleName = r.RoleName,
            Description = r.Description
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.Roles
                .OrderBy(r => r.RoleId)
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
            var r = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (r is null) return NotFound();
            return Ok(ToDto(r));
        }

        [HttpPost]
        public IActionResult Create(CreateRoleDTO dto)
        {
            var r = new Role
            {
                RoleName = dto.RoleName,
                Description = dto.Description
            };

            _context.Roles.Add(r);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = r.RoleId }, ToDto(r));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateRoleDTO dto)
        {
            var r = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (r is null) return NotFound();

            r.RoleName = dto.RoleName;
            r.Description = dto.Description;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchRoleDTO dto)
        {
            var r = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (r is null) return NotFound();

            if (dto.RoleName is not null) r.RoleName = dto.RoleName;
            if (dto.Description is not null) r.Description = dto.Description;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var r = _context.Roles.FirstOrDefault(x => x.RoleId == id);
            if (r is null) return NotFound();

            _context.Roles.Remove(r);
            _context.SaveChanges();
            return NoContent();
        }
    }
}