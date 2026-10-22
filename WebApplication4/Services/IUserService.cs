using WebApplication4.DTO;

namespace WebApplication4.Services;

public interface IUserService : IService<UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>
{
}
