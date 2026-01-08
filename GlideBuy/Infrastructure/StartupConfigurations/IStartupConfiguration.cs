namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	/// <summary>
	/// An object for configuring related services and middleware during startup
	/// </summary>
	public interface IStartupConfiguration
	{
		public void ConfigureServices(IServiceCollection services, IConfiguration configuration);

		public void ConfigureApp(IApplicationBuilder app);

		public StartupOrder Order { get; }
	}
}
