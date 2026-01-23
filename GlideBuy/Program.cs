using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Infrastructure;
using GlideBuy.Data;
using GlideBuy.Services.Orders;
using GlideBuy.Support.Mvc.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureWebApplicationServices(builder.Configuration);

builder.Services.AddDbContext<StoreDbContext>(opts => {
	opts.UseSqlServer(builder.Configuration["ConnectionStrings:OnlineStoreConnection"]);
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<RouteProvider>();
builder.Services.AddSingleton<GenericUrlRouteProvider>();
builder.Services.AddSingleton<ICustomUrlHelper, CustomUrlHelper>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

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

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

SeedData.EnsurePopulated(app);

app.Run();