using GlideBuy.Core.Caching;
using GlideBuy.Core.Configuration;
using GlideBuy.Services.Configuration;
using GlideBuy.Services.Orders;

namespace GlideBuy.Infrastructure.StartupConfigurations
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

			services.AddScoped<IOrderProcessingService, OrderProcessingService>();

			services.AddScoped<ISettingService, SettingService>();

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
