using GlideBuy.Infrastructure;
using GlideBuy.Infrastructure.StartupConfigurations;

namespace GlideBuy.Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static void ConfigureWebApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			// TODO: Configure plugins here in the future.

			// TODO: Add a rate limiter

			// TODO: Configure all services
			TypeFinder finder = new TypeFinder();
			var startupConfigurationClasses = finder.FindClassesByType<IStartupConfiguration>();

			var instances = startupConfigurationClasses
								.Select(startup => (IStartupConfiguration)Activator.CreateInstance(startup))
								.Where(startup => startup != null)
								.OrderBy(startup => (int)(startup.Order));

			foreach (var instance in instances)
			{
				instance.ConfigureServices(services, configuration);
			}

			// TODO: Run startup tasks here.
		}
	}
}
