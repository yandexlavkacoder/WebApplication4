using AutoMapper;
using WebApplication4.DTO;
using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class StockReservationService : IStockReservationService
{
    private readonly IStockReservationRepository _reservationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public StockReservationService(
        IStockReservationRepository reservationRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<(List<StockReservationDTO> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var items = await _reservationRepository.GetAllAsync(page, pageSize, ct);
        var total = await _reservationRepository.CountAsync(ct);
        return (_mapper.Map<List<StockReservationDTO>>(items), total);
    }

    public async Task<StockReservationDTO?> GetByIdAsync(int id, CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id, ct);
        return reservation is null ? null : _mapper.Map<StockReservationDTO>(reservation);
    }

    public async Task<StockReservationDTO> ReserveAsync(CreateStockReservationDTO dto, CancellationToken ct)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException("Количество для резерва должно быть больше нуля.");

        var product = await _productRepository.GetByIdAsync(dto.ProductId, ct)
            ?? throw new InvalidOperationException($"Товар с Id {dto.ProductId} не найден.");

        if (!await _userRepository.ExistsAsync(dto.UserId, ct))
            throw new InvalidOperationException($"Пользователь с Id {dto.UserId} не найден.");

        if (product.IsActive == false)
            throw new InvalidOperationException($"Товар «{product.ProductName}» неактивен и не может быть зарезервирован.");

        if (product.StockQuantity < dto.Quantity)
            throw new InvalidOperationException(
                $"Недостаточно товара «{product.ProductName}» на складе: доступно {product.StockQuantity}, запрошено {dto.Quantity}.");

        if (await _reservationRepository.HasActiveAsync(dto.UserId, dto.ProductId, ct))
            throw new InvalidOperationException("У пользователя уже есть активный резерв на этот товар.");

        product.StockQuantity -= dto.Quantity;

        var reservation = _mapper.Map<StockReservation>(dto);
        reservation.ReservedAt = DateTime.UtcNow;
        await _reservationRepository.AddAsync(reservation, ct);

        await _reservationRepository.SaveChangesAsync(ct);

        var saved = await _reservationRepository.GetByIdAsync(reservation.StockReservationId, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданный резерв.");
        return _mapper.Map<StockReservationDTO>(saved);
    }

    public async Task<StockReservationDTO?> ReleaseAsync(int id, CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetActiveAsync(id, ct);
        if (reservation is null) return null;

        reservation.Product.StockQuantity += reservation.Quantity;
        reservation.ReleasedAt = DateTime.UtcNow;

        await _reservationRepository.SaveChangesAsync(ct);

        return _mapper.Map<StockReservationDTO>(reservation);
    }
}
