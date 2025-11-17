using GlideBuy.Core.Infrastructure;

namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	public class EndpointsStartupConfiguration : IStartupConfiguration
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			// Do nothing
		}

		public void ConfigureApp(IApplicationBuilder app)
		{
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				// Resolve<IRoutePublisher>
				// GetServiceProvider(scope)?.GetService(type);
				// scope = null, type = IRoutePublisher
				// IServiceProvider
				//
				// ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.RequestServices
				// if null -> ServiceProvider
				// ServiceProvider = application.ApplicationServices;


				app.ApplicationServices.GetService<RouteProvider>()?.AddRoutes(endpoints);
			});
		}

		public StartupOrder Order => StartupOrder.Endpoints;

	}
}
