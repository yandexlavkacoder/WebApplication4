using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication4;
using WebApplication4.Mapping;
using WebApplication4.Repositories;
using WebApplication4.Services;

namespace WebApplication4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext — Scoped (по умолчанию для AddDbContext): один экземпляр
            // на HTTP-запрос, поэтому все репозитории одного запроса работают
            // с одним контекстом и один вызов SaveChangesAsync атомарно
            // фиксирует изменения всех сущностей бизнес-операции.
            builder.Services.AddDbContext<ShopDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Репозитории — Scoped: живут ровно один HTTP-запрос и делят
            // Scoped DbContext, что и обеспечивает одну транзакцию сохранения
            // без UnitOfWork и явных BeginTransaction.
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<IStockReservationRepository, StockReservationRepository>();

            // Сервисы — Scoped: соответствуют времени жизни репозиториев и DbContext.
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IStockReservationService, StockReservationService>();

            // AutoMapper: Singleton — профиль компилируется один раз и
            // потокобезопасен, состояние не хранит.
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), typeof(Program));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
