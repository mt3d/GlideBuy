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
			//app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				// EngineContext.Current.Resolve<IRoutePublisher>
				// => GetServiceProvider()?.GetService(typeof(IRoutePubliser));
				//
				// ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.RequestServices
				// if null -> ServiceProvider
				// ServiceProvider = application.ApplicationServices;


				app.ApplicationServices.GetService<RouteProvider>()?.AddRoutes(endpoints);
				app.ApplicationServices.GetService<GenericUrlRouteProvider>()?.AddRoutes(endpoints);
			});
		}

		public StartupOrder Order => StartupOrder.Endpoints;

	}
}
