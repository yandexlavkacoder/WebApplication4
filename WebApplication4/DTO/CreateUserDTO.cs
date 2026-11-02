namespace WebApplication4.DTO;

public class CreateUserDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
}
