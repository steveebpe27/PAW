using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.DataAccess.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ProductDB") ??
    "Server=localhost;Database=ProductDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IPawTaskRepository, PawTaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserActionRepository, UserActionRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
