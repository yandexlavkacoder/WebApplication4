using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ShopDbContext Context;
    private readonly string _keyName;

    public Repository(ShopDbContext context)
    {
        Context = context;
        _keyName = context.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties[0].Name;
    }

    protected virtual IQueryable<T> Query() => Context.Set<T>();

    public async Task<List<T>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        var key = _keyName;
        return await Query()
            .OrderBy(e => EF.Property<int>(e, key))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await Context.Set<T>().CountAsync(ct);

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
    {
        var key = _keyName;
        return await Query().FirstOrDefaultAsync(e => EF.Property<int>(e, key) == id, ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct)
    {
        var key = _keyName;
        return await Context.Set<T>().AnyAsync(e => EF.Property<int>(e, key) == id, ct);
    }

    public int GetId(T entity) =>
        (int)Context.Entry(entity).Property(_keyName).CurrentValue!;

    public async Task AddAsync(T entity, CancellationToken ct) =>
        await Context.Set<T>().AddAsync(entity, ct);

    public void Remove(T entity) => Context.Set<T>().Remove(entity);

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        try
        {
            await Context.SaveChangesAsync(ct);
        }
        catch (Exception ex) when (ex is DbUpdateException or InvalidOperationException)
        {
            throw new InvalidOperationException(
                "Не удалось сохранить изменения: запись связана с другими данными или нарушает уникальность.");
        }
    }
}
