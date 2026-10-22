using AutoMapper;
using WebApplication4;
using WebApplication4.DTO;

namespace WebApplication4.Mapping
{
    /// <summary>
    /// Единый профиль AutoMapper: сущности -> DTO чтения (с вложенными связанными данными)
    /// и DTO записи -> сущности (с Id связанных сущностей). Ручных методов маппинга нет.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ---------- Product ----------
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category));
            CreateMap<CreateProductDTO, Product>()
                .ForMember(d => d.ProductId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore()) // задаётся сервисом/БД
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d => d.OrderItems, opt => opt.Ignore())
                .ForMember(d => d.CartItems, opt => opt.Ignore())
                .ForMember(d => d.StockReservations, opt => opt.Ignore());
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(d => d.ProductId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d => d.OrderItems, opt => opt.Ignore())
                .ForMember(d => d.CartItems, opt => opt.Ignore())
                .ForMember(d => d.StockReservations, opt => opt.Ignore());
            // Patch: только заданные (не null) поля переносятся в сущность.
            CreateMap<PatchProductDTO, Product>()
                .ForMember(d => d.ProductId, opt => opt.Ignore())
                .ForMember(d => d.ProductName, opt => opt.Condition(s => s.ProductName != null))
                .ForMember(d => d.Description, opt => opt.Condition(s => s.Description != null))
                .ForMember(d => d.Price, opt => opt.Condition(s => s.Price != null))
                .ForMember(d => d.StockQuantity, opt => opt.Condition(s => s.StockQuantity != null))
                .ForMember(d => d.CategoryId, opt => opt.Condition(s => s.CategoryId != null))
                .ForMember(d => d.IsActive, opt => opt.Condition(s => s.IsActive != null))
                .ForMember(d => d.CreatedAt, opt => opt.Condition(s => s.CreatedAt != null))
                .ForMember(d => d.ImagePath, opt => opt.Condition(s => s.ImagePath != null))
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d => d.OrderItems, opt => opt.Ignore())
                .ForMember(d => d.CartItems, opt => opt.Ignore())
                .ForMember(d => d.StockReservations, opt => opt.Ignore());

            // ---------- Category ----------
            CreateMap<Category, CategoryDTO>()
                .ForMember(d => d.Subcategories, opt => opt.MapFrom(s => s.InverseParentCategory))
                .ForMember(d => d.ParentCategory, opt => opt.MapFrom(s => s.ParentCategory))
                .ForMember(d => d.Products, opt => opt.MapFrom(s => s.Products));
            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(d => d.CategoryId, opt => opt.Ignore())
                .ForMember(d => d.ParentCategory, opt => opt.Ignore())
                .ForMember(d => d.InverseParentCategory, opt => opt.Ignore())
                .ForMember(d => d.Products, opt => opt.Ignore());
            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(d => d.CategoryId, opt => opt.Ignore())
                .ForMember(d => d.ParentCategory, opt => opt.Ignore())
                .ForMember(d => d.InverseParentCategory, opt => opt.Ignore())
                .ForMember(d => d.Products, opt => opt.Ignore());
            CreateMap<PatchCategoryDTO, Category>()
                .ForMember(d => d.CategoryId, opt => opt.Ignore())
                .ForMember(d => d.CategoryName, opt => opt.Condition(s => s.CategoryName != null))
                .ForMember(d => d.Description, opt => opt.Condition(s => s.Description != null))
                .ForMember(d => d.ParentCategoryId, opt => opt.Condition(s => s.ParentCategoryId != null))
                .ForMember(d => d.ParentCategory, opt => opt.Ignore())
                .ForMember(d => d.InverseParentCategory, opt => opt.Ignore())
                .ForMember(d => d.Products, opt => opt.Ignore());

            // ---------- Order ----------
            CreateMap<Order, OrderDTO>()
                .ForMember(d => d.User, opt => opt.MapFrom(s => s.User))
                .ForMember(d => d.OrderItems, opt => opt.MapFrom(s => s.OrderItems));
            CreateMap<CreateOrderDTO, Order>()
                .ForMember(d => d.OrderId, opt => opt.Ignore())
                .ForMember(d => d.OrderDate, opt => opt.Ignore()) // getdate() в БД
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.OrderItems, opt => opt.Ignore());
            CreateMap<UpdateOrderDTO, Order>()
                .ForMember(d => d.OrderId, opt => opt.Ignore())
                .ForMember(d => d.OrderDate, opt => opt.Condition(s => s.OrderDate != null))
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.OrderItems, opt => opt.Ignore());
            CreateMap<PatchOrderDTO, Order>()
                .ForMember(d => d.OrderId, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Condition(s => s.UserId != null))
                .ForMember(d => d.OrderDate, opt => opt.Condition(s => s.OrderDate != null))
                .ForMember(d => d.TotalAmount, opt => opt.Condition(s => s.TotalAmount != null))
                .ForMember(d => d.Status, opt => opt.Condition(s => s.Status != null))
                .ForMember(d => d.ShippingAddress, opt => opt.Condition(s => s.ShippingAddress != null))
                .ForMember(d => d.PaymentMethod, opt => opt.Condition(s => s.PaymentMethod != null))
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.OrderItems, opt => opt.Ignore());

            // ---------- OrderItem ----------
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.Product, opt => opt.MapFrom(s => s.Product));
            CreateMap<CreateOrderItemDTO, OrderItem>()
                .ForMember(d => d.OrderItemId, opt => opt.Ignore())
                .ForMember(d => d.Order, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());
            CreateMap<UpdateOrderItemDTO, OrderItem>()
                .ForMember(d => d.OrderItemId, opt => opt.Ignore())
                .ForMember(d => d.Order, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());
            CreateMap<PatchOrderItemDTO, OrderItem>()
                .ForMember(d => d.OrderItemId, opt => opt.Ignore())
                .ForMember(d => d.OrderId, opt => opt.Condition(s => s.OrderId != null))
                .ForMember(d => d.ProductId, opt => opt.Condition(s => s.ProductId != null))
                .ForMember(d => d.Quantity, opt => opt.Condition(s => s.Quantity != null))
                .ForMember(d => d.UnitPrice, opt => opt.Condition(s => s.UnitPrice != null))
                .ForMember(d => d.Order, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());

            // ---------- User ----------
            CreateMap<User, UserDTO>()
                .ForMember(d => d.Role, opt => opt.MapFrom(s => s.Role));
            CreateMap<CreateUserDTO, User>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore()) // getdate() в БД
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Orders, opt => opt.Ignore())
                .ForMember(d => d.Cart, opt => opt.Ignore())
                .ForMember(d => d.StockReservations, opt => opt.Ignore());
            CreateMap<UpdateUserDTO, User>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Condition(s => s.CreatedAt != null))
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Orders, opt => opt.Ignore())
                .ForMember(d => d.Cart, opt => opt.Ignore())
                .ForMember(d => d.StockReservations, opt => opt.Ignore());
            CreateMap<PatchUserDTO, User>()
                .ForMember(d => d.UserId, opt => opt.Ignore())
                .ForMember(d => d.Username, opt => opt.Condition(s => s.Username != null))
                .ForMember(d => d.PasswordHash, opt => opt.Condition(s => s.PasswordHash != null))
                .ForMember(d => d.Email, opt => opt.Condition(s => s.Email != null))
                .ForMember(d => d.CreatedAt, opt => opt.Condition(s => s.CreatedAt != null))
                .ForMember(d => d.RoleId, opt => opt.Condition(s => s.RoleId != null))
                .ForMember(d => d.IsActive, opt => opt.Condition(s => s.IsActive != null))
                .ForMember(d => d.Role, opt => opt.Ignore())
                .ForMember(d => d.Orders, opt => opt.Ignore())
                .ForMember(d => d.Cart, opt => opt.Ignore())
                .ForMember(d => d.StockReservations, opt => opt.Ignore());

            // ---------- Role ----------
            CreateMap<Role, RoleDTO>();
            CreateMap<CreateRoleDTO, Role>()
                .ForMember(d => d.RoleId, opt => opt.Ignore())
                .ForMember(d => d.Users, opt => opt.Ignore());
            CreateMap<UpdateRoleDTO, Role>()
                .ForMember(d => d.RoleId, opt => opt.Ignore())
                .ForMember(d => d.Users, opt => opt.Ignore());
            CreateMap<PatchRoleDTO, Role>()
                .ForMember(d => d.RoleId, opt => opt.Ignore())
                .ForMember(d => d.RoleName, opt => opt.Condition(s => s.RoleName != null))
                .ForMember(d => d.Description, opt => opt.Condition(s => s.Description != null))
                .ForMember(d => d.Users, opt => opt.Ignore());

            // ---------- Cart ----------
            CreateMap<Cart, CartDTO>()
                .ForMember(d => d.User, opt => opt.MapFrom(s => s.User))
                .ForMember(d => d.CartItems, opt => opt.MapFrom(s => s.CartItems));
            CreateMap<CreateCartDTO, Cart>()
                .ForMember(d => d.CartId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.CartItems, opt => opt.Ignore());
            CreateMap<UpdateCartDTO, Cart>()
                .ForMember(d => d.CartId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Condition(s => s.CreatedAt != null))
                .ForMember(d => d.UpdatedAt, opt => opt.Condition(s => s.UpdatedAt != null))
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.CartItems, opt => opt.Ignore());
            CreateMap<PatchCartDTO, Cart>()
                .ForMember(d => d.CartId, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Condition(s => s.UserId != null))
                .ForMember(d => d.CreatedAt, opt => opt.Condition(s => s.CreatedAt != null))
                .ForMember(d => d.UpdatedAt, opt => opt.Condition(s => s.UpdatedAt != null))
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.CartItems, opt => opt.Ignore());

            // ---------- CartItem ----------
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(d => d.Product, opt => opt.MapFrom(s => s.Product));
            CreateMap<CreateCartItemDTO, CartItem>()
                .ForMember(d => d.CartItemId, opt => opt.Ignore())
                .ForMember(d => d.AddedAt, opt => opt.Ignore()) // getdate() в БД
                .ForMember(d => d.Cart, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());
            CreateMap<UpdateCartItemDTO, CartItem>()
                .ForMember(d => d.CartItemId, opt => opt.Ignore())
                .ForMember(d => d.AddedAt, opt => opt.Condition(s => s.AddedAt != null))
                .ForMember(d => d.Cart, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());
            CreateMap<PatchCartItemDTO, CartItem>()
                .ForMember(d => d.CartItemId, opt => opt.Ignore())
                .ForMember(d => d.CartId, opt => opt.Condition(s => s.CartId != null))
                .ForMember(d => d.ProductId, opt => opt.Condition(s => s.ProductId != null))
                .ForMember(d => d.Quantity, opt => opt.Condition(s => s.Quantity != null))
                .ForMember(d => d.AddedAt, opt => opt.Condition(s => s.AddedAt != null))
                .ForMember(d => d.Cart, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());

            // ---------- StockReservation (факт бизнес-операции) ----------
            CreateMap<StockReservation, StockReservationDTO>()
                .ForMember(d => d.User, opt => opt.MapFrom(s => s.User))
                .ForMember(d => d.Product, opt => opt.MapFrom(s => s.Product));
            CreateMap<CreateStockReservationDTO, StockReservation>()
                .ForMember(d => d.StockReservationId, opt => opt.Ignore())
                .ForMember(d => d.ReservedAt, opt => opt.Ignore()) // задаёт сервис
                .ForMember(d => d.ReleasedAt, opt => opt.Ignore())
                .ForMember(d => d.User, opt => opt.Ignore())
                .ForMember(d => d.Product, opt => opt.Ignore());
        }
    }
}
