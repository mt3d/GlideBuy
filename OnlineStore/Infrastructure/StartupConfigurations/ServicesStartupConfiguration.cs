using GlideBuy.Core.Caching;

namespace GlideBuy.Infrastructure.StartupConfigurations
{
	public class ServicesStartupConfiguration : IStartupConfiguration
	{
		public StartupOrder Order => throw new NotImplementedException();

		public void ConfigureApp(IApplicationBuilder app)
		{
		}

		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
			services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
			services.AddSingleton<ICacheKeyBuilder, CacheKeyBuilder>();

			//services.AddScoped<ISettingService, SettingService>();

			//TypeFinder typeFinder = new TypeFinder();
			//var settings = typeFinder.FindClassesByType<ISettings>(false).ToList();

			//foreach (var setting in settings)
			//{
			//	services.AddScoped(setting, serviceProvider =>
			//	{
			//		// Block until completion.
			//		//return serviceProvider.GetRequiredService<ISettingService>().LoadSettingsAsync(setting).Result;
			//	});
			//}
		}
	}
}
