using WebApplication4.Authorization;

namespace WebApplication4.DTO;

public class UserDTO : IOwnedResource
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime? CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public RoleDTO Role { get; set; } = null!;

    int IOwnedResource.OwnerUserId => UserId;
}
