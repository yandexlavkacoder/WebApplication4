using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    /// <summary>
    /// Read-only сервис резервов: список с пагинацией и получение по id
    /// (с вложенными User и Product).
    /// </summary>
    public class StockReservationService : IStockReservationService
    {
        private readonly IStockReservationRepository _repository;
        private readonly IMapper _mapper;

        public StockReservationService(IStockReservationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResult<StockReservationDTO>> GetPagedAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var totalCount = await _repository.CountAsync();
            var items = await _repository.GetAllAsync(
                orderBy: q => q.OrderByDescending(r => r.StockReservationId),
                skip: (page - 1) * pageSize,
                take: pageSize);

            return new PagedResult<StockReservationDTO>
            {
                Items = _mapper.Map<System.Collections.Generic.IReadOnlyList<StockReservationDTO>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<StockReservationDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<StockReservationDTO>(entity);
        }
    }
}
