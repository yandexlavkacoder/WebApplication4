namespace WebApplication4.DTO
{
    public class CreateUserDTO
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}