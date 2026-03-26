using GlideBuy.Core.Infrastructure;
using GlideBuy.Data;
using GlideBuy.Services.Installation;
using GlideBuy.Services.Orders;
using GlideBuy.Support.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

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
using (var scope = app.Services.CreateScope())
{
    var installationService = scope.ServiceProvider.GetRequiredService<IInstallationService>();
    await installationService.InstallAsync();
}

app.Run();