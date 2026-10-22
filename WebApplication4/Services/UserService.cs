using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    public class UserService
        : Service<User, UserDTO, CreateUserDTO, UpdateUserDTO, PatchUserDTO>,
          IUserService
    {
        public UserService(IUserRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
