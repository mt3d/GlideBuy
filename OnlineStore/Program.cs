using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Identity;
using OnlineStore.Infrastructure;
using OnlineStore.Models.Repositories;
using OnlineStore.Services.ProductCatalog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureWebApplicationServices(builder.Configuration);

builder.Services.AddDbContext<StoreDbContext>(opts => {
	opts.UseSqlServer(builder.Configuration["ConnectionStrings:OnlineStoreConnection"]);
});

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<AppIdentityDbContext>(opts => {
	opts.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddSingleton<RouteProvider>();

var app = builder.Build();

if (app.Environment.IsProduction())
{
	app.UseExceptionHandler("/error");
}

app.UseRequestLocalization(opts =>
{
	opts.AddSupportedCultures("en-US")
	.AddSupportedUICultures("en-US")
	.SetDefaultCulture("en-US");
});

app.UseStaticFiles();
app.UseSession();

app.ConfigurePipeline();

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

SeedData.EnsurePopulated(app);
IdentitySeedData.EnsurePopulated(app);

app.Run();