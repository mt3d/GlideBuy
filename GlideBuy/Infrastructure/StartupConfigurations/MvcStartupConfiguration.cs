namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
    public class MvcStartupConfiguration : IStartupConfiguration
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvcServices();

            // TODO: Add Web Encoders
        }

        public void ConfigureApp(IApplicationBuilder app)
        {
            // No need to configure anything
        }

        public StartupOrder Order => StartupOrder.Mvc;
    }
}
