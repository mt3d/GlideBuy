using GlideBuy.Core.Caching;
using GlideBuy.Core.Configuration;
using GlideBuy.Data.Repositories;
using GlideBuy.Services.Catalog;
using GlideBuy.Services.Common;
using GlideBuy.Services.Configuration;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Orders;
using GlideBuy.Services.Seo;
using GlideBuy.Services.Shipping;
using GlideBuy.Support;
using GlideBuy.Support.Mvc.Routing;
using GlideBuy.Web.Factories;

namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	public class ServicesStartupConfiguration : IStartupConfiguration
	{
		public StartupOrder Order => StartupOrder.Services;

		public void ConfigureApp(IApplicationBuilder app)
		{
		}

		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
			services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
			services.AddSingleton<ICacheKeyBuilder, MemoryCacheManager>();

			services.AddScoped<IWorkContext, WebWorkContext>();

			services.AddScoped<IOrderProcessingService, OrderProcessingService>();

			services.AddScoped<ISettingService, SettingService>();
			services.AddScoped<IAddressService, AddressService>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IOrderTotalCalculationService, OrderTotalCalculationService>();

			services.AddScoped<ProductRepository>();
			services.AddScoped<OrderRepository>();
			services.AddScoped<ManufacturerRepository>();

			services.AddScoped<IShoppingCartModelsFactory, ShoppingCartModelsFactory>();
			services.AddScoped<ICheckoutModelFactory, CheckoutModelFactory>();
			services.AddScoped<IAddressModelFactory, AddressModelFactory>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IShippingService, ShippingService>();
			services.AddScoped<ICatalogModelFactory, CatalogModelFactory>();
			services.AddScoped<IUrlRecordService, UrlRecordService>();
			services.AddScoped<ICategoryService,  CategoryService>();
			services.AddScoped<ICommonModelFactory, CommonModelFactory>();

			// TODO: Check if the database is installed. Why?
			services.AddScoped<SlugRouteTransformer>();

			// TODO: Use Singleton.
			TypeFinder typeFinder = new TypeFinder();
			var settings = typeFinder.FindClassesByType<ISettings>(false).ToList();
			foreach (var setting in settings)
			{
				services.AddScoped(setting, serviceProvider =>
				{
					// Block until completion.
					return serviceProvider.GetRequiredService<ISettingService>().LoadSettingsAsync(setting).Result;
				});
			}
		}
	}
}
