using GlideBuy.Infrastructure;
using GlideBuy.Infrastructure.StartupConfigurations;

namespace GlideBuy.Infrastructure
{
	public static class ApplicationBuilderExtensions
	{
		public static void ConfigurePipeline(this IApplicationBuilder app)
		{
			TypeFinder finder = new TypeFinder();
			IEnumerable<Type> startupConfigurationClasses = finder.FindClassesByType<IStartupConfiguration>();

			var instances = startupConfigurationClasses
								.Select(startup => (IStartupConfiguration)Activator.CreateInstance(startup))
								.Where(startup => startup != null)
								.OrderBy(startup => (int)(startup.Order));

			foreach (var instance in instances)
			{
				instance?.ConfigureApp(app);
			}
		}
	}
}
