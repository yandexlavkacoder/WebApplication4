namespace WebApplication4.Services;

public interface IService<TDto, TCreateDto, TUpdateDto, TPatchDto> where TDto : class
{
    Task<(List<TDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<TDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<TDto> CreateAsync(TCreateDto dto, CancellationToken ct);
    Task<TDto?> UpdateAsync(int id, TUpdateDto dto, CancellationToken ct);
    Task<TDto?> PatchAsync(int id, TPatchDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
