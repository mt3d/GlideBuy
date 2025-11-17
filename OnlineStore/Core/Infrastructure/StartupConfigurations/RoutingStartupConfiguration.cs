namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	public class RoutingStartupConfiguration : IStartupConfiguration
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			// Do nothing
		}

		public void ConfigureApp(IApplicationBuilder app)
		{
			app.UseRouting();
		}

		public StartupOrder Order => StartupOrder.Routing;
	}
}
