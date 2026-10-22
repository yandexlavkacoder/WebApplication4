namespace WebApplication4.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<T?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> ExistsAsync(int id, CancellationToken ct);
    int GetId(T entity);
    Task AddAsync(T entity, CancellationToken ct);
    void Remove(T entity);
    Task SaveChangesAsync(CancellationToken ct);
}
