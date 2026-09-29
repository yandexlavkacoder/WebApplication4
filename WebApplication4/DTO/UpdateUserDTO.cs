using System;

namespace WebApplication4.DTO
{
    public class UpdateUserDTO
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}