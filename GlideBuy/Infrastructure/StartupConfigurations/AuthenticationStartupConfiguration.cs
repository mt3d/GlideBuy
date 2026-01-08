namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	public class AuthenticationStartupConfiguration : IStartupConfiguration
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			// Do nothing
		}

		public void ConfigureApp(IApplicationBuilder app)
		{
			app.UseAuthentication();
			app.UseAuthorization();
		}

		public StartupOrder Order => StartupOrder.Authentication;
	}
}
