namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	public class MvcStartupConfiguration : IStartupConfiguration
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllersWithViews()
				.AddRazorOptions(options =>
				{
					options.ViewLocationFormats.Add("/{0}.cshtml");
				});

			// TODO: Configure AddSessionStateTempDataProvider or AddCookieTempDataProvider

			services.AddRazorPages();

			// TODO: AddMvcOptions
			// TODO: Add fluent validation
			// TODO: Add all validators
			// TODO: AddControllersAsServices()

			// TODO: Add Web Encoders
		}

		public void ConfigureApp(IApplicationBuilder app)
		{
			// No need to configure anything
		}

		public StartupOrder Order => StartupOrder.Mvc;
	}
}
