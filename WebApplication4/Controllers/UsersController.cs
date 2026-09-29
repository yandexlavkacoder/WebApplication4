using Microsoft.AspNetCore.Mvc;
using WebApplication4.DTO;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public UsersController(ShopDbContext context)
        {
            _context = context;
        }

        private static UserDTO ToDto(User u) => new UserDTO
        {
            UserId = u.UserId,
            Username = u.Username,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            RoleId = u.RoleId,
            IsActive = u.IsActive
        };

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var items = _context.Users
                .OrderBy(u => u.UserId)
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
            var u = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (u is null) return NotFound();
            return Ok(ToDto(u));
        }

        [HttpPost]
        public IActionResult Create(CreateUserDTO dto)
        {
            var u = new User
            {
                Username = dto.Username,
                PasswordHash = dto.PasswordHash,
                Email = dto.Email,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(u);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = u.UserId }, ToDto(u));
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateUserDTO dto)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (u is null) return NotFound();

            u.Username = dto.Username;
            u.Email = dto.Email;
            u.CreatedAt = dto.CreatedAt;
            u.RoleId = dto.RoleId;
            u.IsActive = dto.IsActive;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchUserDTO dto)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (u is null) return NotFound();

            if (dto.Username is not null) u.Username = dto.Username;
            if (dto.PasswordHash is not null) u.PasswordHash = dto.PasswordHash;
            if (dto.Email is not null) u.Email = dto.Email;
            if (dto.CreatedAt is not null) u.CreatedAt = dto.CreatedAt.Value;
            if (dto.RoleId is not null) u.RoleId = dto.RoleId.Value;
            if (dto.IsActive is not null) u.IsActive = dto.IsActive.Value;

            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (u is null) return NotFound();

            _context.Users.Remove(u);
            _context.SaveChanges();
            return NoContent();
        }
    }
}