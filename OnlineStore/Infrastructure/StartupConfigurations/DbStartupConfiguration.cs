using GlideBuy.Data;

namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
	public class DbStartupConfiguration : IStartupConfiguration
	{
		public StartupOrder Order => StartupOrder.Database;

		public void ConfigureApp(IApplicationBuilder app)
		{
		}

		public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
		}
	}
}
