using PAW.Web.Services;
using Microsoft.AspNetCore.DataProtection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// Optional location for restricted environments; normal Visual Studio runs use the standard store.
if (builder.Configuration["DataProtection:KeysPath"] is string keysPath)
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(keysPath));
builder.Services.Configure<Microsoft.AspNetCore.Builder.RequestLocalizationOptions>(options =>
    options.SetDefaultCulture("en-US").AddSupportedCultures("en-US").AddSupportedUICultures("en-US"));
builder.Services.AddHttpClient<ApiClient>(client => client.BaseAddress = new Uri(
    builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7038/"));
builder.Services.AddScoped<LookupService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPawTaskService, PawTaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserActionService, UserActionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddHttpClient<TipoCambioClient>(client =>
{
    client.BaseAddress = new Uri("https://api.hacienda.go.cr/");
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestVersion = System.Net.HttpVersion.Version11;
    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
    client.DefaultRequestHeaders.UserAgent.ParseAdd("PAW.Web/1.0");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
});
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
