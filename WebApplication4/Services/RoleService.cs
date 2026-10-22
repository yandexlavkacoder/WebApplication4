using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class RoleService : Service<Role, RoleDTO, CreateRoleDTO, UpdateRoleDTO, PatchRoleDTO>, IRoleService
{
    public RoleService(IRoleRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
    }
}
