namespace WebApplication4.DTO;

public class LoginResponseDTO
{
    public string AccessToken { get; set; } = "";
    public int ExpiresInMinutes { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string Role { get; set; } = "";
}
