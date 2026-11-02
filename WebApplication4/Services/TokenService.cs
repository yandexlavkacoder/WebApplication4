using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication4.Services;

public class TokenService : ITokenService
{
    private readonly IConfigurationSection _jwt;

    public TokenService(IConfiguration config)
    {
        _jwt = config.GetSection("Jwt");
    }

    public int ExpiresInMinutes => int.Parse(_jwt["ExpiresMinutes"]!);

    public string CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.RoleName),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt["Issuer"],
            audience: _jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
