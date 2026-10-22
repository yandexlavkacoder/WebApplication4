namespace WebApplication4.DTO
{
    /// <summary>
    /// DTO чтения роли — со списком пользователей (один-ко-многим).
    /// </summary>
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
