namespace WebApplication4.Services;

public interface ITokenService
{
    int ExpiresInMinutes { get; }
    string CreateAccessToken(User user);
}
