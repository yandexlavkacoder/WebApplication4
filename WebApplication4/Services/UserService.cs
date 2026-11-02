using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class UserService : Service<User, UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>, IUserService
{
    public const string DefaultRoleName = "User";

    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly PasswordHasher<User> _hasher = new();

    public UserService(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _userRepository = repository;
        _roleRepository = roleRepository;
    }

    public async Task<User> RegisterAsync(RegisterDTO dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email))
            throw new InvalidOperationException("Логин и email обязательны.");
        if (dto.Password.Length < 6)
            throw new InvalidOperationException("Пароль должен быть не короче 6 символов.");
        if (await _userRepository.UsernameOrEmailTakenAsync(dto.Username, dto.Email, ct))
            throw new InvalidOperationException("Пользователь с таким логином или email уже существует.");

        var role = await _roleRepository.GetByNameAsync(DefaultRoleName, ct)
            ?? throw new InvalidOperationException($"В базе нет роли «{DefaultRoleName}».");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            RoleId = role.RoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };
        user.PasswordHash = _hasher.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);

        user.Role = role;
        return user;
    }

    public async Task<User?> ValidateCredentialsAsync(string username, string password, CancellationToken ct)
    {
        var user = await _userRepository.GetByUsernameAsync(username, ct);
        if (user is null || !user.IsActive) return null;

        try
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Failed ? null : user;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    protected override void BeforeCreate(User user, CreateUserDTO dto)
    {
        if (dto.Password.Length < 6)
            throw new InvalidOperationException("Пароль должен быть не короче 6 символов.");
        user.PasswordHash = _hasher.HashPassword(user, dto.Password);
        user.CreatedAt = DateTime.UtcNow;
    }

    protected override async Task ValidateAsync(User user, CancellationToken ct)
    {
        if (!await _roleRepository.ExistsAsync(user.RoleId, ct))
            throw new InvalidOperationException($"Роль с Id {user.RoleId} не найдена.");
    }
}
