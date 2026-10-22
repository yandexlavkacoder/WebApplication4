using System.Threading.Tasks;
using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services
{
    /// <summary>
    /// Сервис товаров: CRUD + бизнес-операции резерва/снятия резерва.
    /// Бизнес-операции меняют две связанные сущности (Product и StockReservation)
    /// и сохраняют изменения одним вызовом SaveChangesAsync — без UnitOfWork
    /// и без явных транзакций (репозитории одного scope делят один DbContext).
    /// </summary>
    public class ProductService
        : Service<Product, ProductDTO, CreateProductDTO, UpdateProductDTO, PatchProductDTO>,
          IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStockReservationRepository _reservationRepository;

        public ProductService(
            IProductRepository repository,
            IUserRepository userRepository,
            IStockReservationRepository reservationRepository,
            IMapper mapper)
            : base(repository, mapper)
        {
            _productRepository = repository;
            _userRepository = userRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<StockReservationDTO> ReserveStockAsync(CreateStockReservationDTO dto)
        {
            if (dto.Quantity <= 0)
                throw new BusinessException("Количество для резерва должно быть положительным.");

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product is null)
                throw new BusinessException($"Товар с id = {dto.ProductId} не найден.");

            if (product.IsActive == false)
                throw new BusinessException("Товар неактивен и не может быть зарезервирован.");

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user is null)
                throw new BusinessException($"Пользователь с id = {dto.UserId} не найден.");

            // Проверка бизнес-условия: свободного количества должно хватать.
            if (product.StockQuantity < dto.Quantity)
                throw new BusinessException(
                    $"Недостаточно товара «{product.ProductName}» на складе: доступно {product.StockQuantity}, запрошено {dto.Quantity}.");

            var active = await _reservationRepository.GetActiveByUserAndProductAsync(dto.UserId, dto.ProductId);
            if (active is not null)
                throw new BusinessException(
                    $"У пользователя уже есть активный резерв (id = {active.StockReservationId}) на этот товар.");

            // 1-я сущность: уменьшаем остаток товара.
            product.StockQuantity -= dto.Quantity;

            // 2-я сущность: создаём запись о факте операции (кто/что/когда).
            var reservation = new StockReservation
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                ReservedAt = DateTime.UtcNow,
                ReleasedAt = null
            };
            await _reservationRepository.AddAsync(reservation);

            // Один SaveChangesAsync на обе сущности.
            await _reservationRepository.SaveChangesAsync();

            var saved = await _reservationRepository.GetByIdAsync(reservation.StockReservationId);
            return Mapper.Map<StockReservationDTO>(saved ?? reservation);
        }

        public async Task<StockReservationDTO> ReleaseStockAsync(int stockReservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(stockReservationId);
            if (reservation is null)
                throw new BusinessException($"Резерв с id = {stockReservationId} не найден.");

            if (reservation.ReleasedAt is not null)
                throw new BusinessException($"Резерв с id = {stockReservationId} уже закрыт.");

            var product = await _productRepository.GetByIdAsync(reservation.ProductId)
                ?? throw new BusinessException($"Товар с id = {reservation.ProductId} не найден.");

            // Обратная операция: возвращаем количество на склад…
            product.StockQuantity += reservation.Quantity;

            // …и закрываем запись о событии.
            reservation.ReleasedAt = DateTime.UtcNow;

            // Один SaveChangesAsync на обе сущности.
            await _reservationRepository.SaveChangesAsync();

            return Mapper.Map<StockReservationDTO>(reservation);
        }
    }
}
