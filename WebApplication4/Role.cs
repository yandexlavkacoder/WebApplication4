namespace WebApplication4;

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public string? Description { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
