using AutoMapper;
using WebApplication4.DTO;

namespace WebApplication4.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.CategoryName));
        CreateMap<Category, CategoryDTO>()
            .ForMember(d => d.ParentCategoryName, opt => opt.MapFrom(s => s.ParentCategory != null ? s.ParentCategory.CategoryName : null))
            .ForMember(d => d.Products, opt => opt.MapFrom(s => s.Products.Select(p => p.ProductName)));
        CreateMap<Order, OrderDTO>();
        CreateMap<OrderItem, OrderItemDTO>();
        CreateMap<User, UserDTO>();
        CreateMap<Role, RoleDTO>();
        CreateMap<Cart, CartDTO>();
        CreateMap<CartItem, CartItemDTO>();
        CreateMap<StockReservation, StockReservationDTO>();

        CreateMap<CreateProductDTO, Product>();
        CreateMap<UpdateProductDTO, Product>();
        CreateMap<PatchProductDTO, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateCategoryDTO, Category>();
        CreateMap<UpdateCategoryDTO, Category>();
        CreateMap<PatchCategoryDTO, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateOrderDTO, Order>();
        CreateMap<UpdateOrderDTO, Order>();
        CreateMap<PatchOrderDTO, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateOrderItemDTO, OrderItem>();
        CreateMap<UpdateOrderItemDTO, OrderItem>();
        CreateMap<PatchOrderItemDTO, OrderItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateUserDTO, User>();
        CreateMap<UpdateUserDTO, User>();
        CreateMap<PatchUserDTO, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateRoleDTO, Role>();
        CreateMap<UpdateRoleDTO, Role>();
        CreateMap<PatchRoleDTO, Role>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateCartDTO, Cart>();
        CreateMap<UpdateCartDTO, Cart>();
        CreateMap<PatchCartDTO, Cart>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateCartItemDTO, CartItem>();
        CreateMap<UpdateCartItemDTO, CartItem>();
        CreateMap<PatchCartItemDTO, CartItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

        CreateMap<CreateStockReservationDTO, StockReservation>();
    }
}
