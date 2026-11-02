using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebApplication4.DTO;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto, cancellationToken);
            return Ok(CreateResponse(user));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [EnableRateLimiting("LoginPolicy")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto, CancellationToken cancellationToken)
    {
        var user = await _userService.ValidateCredentialsAsync(dto.Username, dto.Password, cancellationToken);
        if (user is null)
            return Unauthorized(new { error = "Неверный логин или пароль." });

        return Ok(CreateResponse(user));
    }

    private LoginResponseDTO CreateResponse(User user) => new()
    {
        AccessToken = _tokenService.CreateAccessToken(user),
        ExpiresInMinutes = _tokenService.ExpiresInMinutes,
        UserId = user.UserId,
        Username = user.Username,
        Role = user.Role.RoleName,
    };
}
