using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface IUserService : IService<UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>
{
    Task<User> RegisterAsync(RegisterDTO dto, CancellationToken ct);
    Task<User?> ValidateCredentialsAsync(string username, string password, CancellationToken ct);
}
