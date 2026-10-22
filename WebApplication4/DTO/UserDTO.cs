using System;
using System.Collections.Generic;

namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения пользователя — с вложенной ролью (многие-к-одному).
    /// </summary>
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        /// <summary>Роль пользователя.</summary>
        public RoleDTO? Role { get; set; }
    }
}
