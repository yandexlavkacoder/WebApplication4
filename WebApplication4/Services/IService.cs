using System.Threading.Tasks;
using WebApplication4.DTO;

namespace WebApplication4.Services
{
    /// <summary>
    /// Общий контракт CRUD-сервиса. Возвращает DTO и результат-обёртку вместо HTTP-типов:
    /// слой сервиса не зависит от ASP.NET Core.
    /// </summary>
    public interface IService<TDto, TCreateDto, TUpdateDto, TPatchDto>
    {
        Task<PagedResult<TDto>> GetPagedAsync(int page, int pageSize);

        Task<TDto?> GetByIdAsync(int id);

        Task<TDto> CreateAsync(TCreateDto dto);

        Task<TDto?> UpdateAsync(int id, TUpdateDto dto);

        Task<TDto?> PatchAsync(int id, TPatchDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
