using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class UserService : Service<User, UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>, IUserService
{
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository repository, IRoleRepository roleRepository, IMapper mapper)
        : base(repository, mapper)
    {
        _roleRepository = roleRepository;
    }

    protected override async Task ValidateAsync(User user, CancellationToken ct)
    {
        if (!await _roleRepository.ExistsAsync(user.RoleId, ct))
            throw new InvalidOperationException($"Роль с Id {user.RoleId} не найдена.");
    }
}
