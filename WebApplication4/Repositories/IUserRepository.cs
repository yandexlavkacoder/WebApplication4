namespace WebApplication4.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<bool> UsernameOrEmailTakenAsync(string username, string email, CancellationToken ct);
}
