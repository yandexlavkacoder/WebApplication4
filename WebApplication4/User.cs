using System;
using System.Collections.Generic;

namespace WebApplication4;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;
}
