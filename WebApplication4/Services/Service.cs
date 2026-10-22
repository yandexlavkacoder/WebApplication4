using AutoMapper;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public abstract class Service<TEntity, TDto, TCreateDto, TUpdateDto, TPatchDto>
    : IService<TDto, TCreateDto, TUpdateDto, TPatchDto>
    where TEntity : class
    where TDto : class
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;

    protected Service(IRepository<TEntity> repository, IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public async Task<(List<TDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var items = await Repository.GetAllAsync(page, pageSize, ct);
        var total = await Repository.CountAsync(ct);
        return (Mapper.Map<List<TDto>>(items), total);
    }

    public async Task<TDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await Repository.GetByIdAsync(id, ct);
        return entity is null ? null : Mapper.Map<TDto>(entity);
    }

    public async Task<TDto> CreateAsync(TCreateDto dto, CancellationToken ct)
    {
        var entity = Mapper.Map<TEntity>(dto);
        await ValidateAsync(entity, ct);

        await Repository.AddAsync(entity, ct);
        await Repository.SaveChangesAsync(ct);

        return await GetByIdAsync(Repository.GetId(entity), ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
    }

    public async Task<TDto?> UpdateAsync(int id, TUpdateDto dto, CancellationToken ct)
    {
        var entity = await Repository.GetByIdAsync(id, ct);
        if (entity is null) return null;

        Mapper.Map(dto, entity);
        await ValidateAsync(entity, ct);
        await Repository.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task<TDto?> PatchAsync(int id, TPatchDto dto, CancellationToken ct)
    {
        var entity = await Repository.GetByIdAsync(id, ct);
        if (entity is null) return null;

        Mapper.Map(dto, entity);
        await ValidateAsync(entity, ct);
        await Repository.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await Repository.GetByIdAsync(id, ct);
        if (entity is null) return false;

        Repository.Remove(entity);
        await Repository.SaveChangesAsync(ct);
        return true;
    }

    protected virtual Task ValidateAsync(TEntity entity, CancellationToken ct) => Task.CompletedTask;
}
