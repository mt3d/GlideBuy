using FluentValidation;
using FluentValidation.AspNetCore;
using GlideBuy.Core.Infrastructure.StartupConfigurations;
using GlideBuy.Validators.Customer;

namespace GlideBuy.Core.Infrastructure
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
                                .OrderBy(startup => (int)startup.Order);

            foreach (var instance in instances)
            {
                instance.ConfigureServices(services, configuration);
            }

            // TODO: Run startup tasks here.
        }

        public static IMvcBuilder AddMvcServices(this IServiceCollection services)
        {
            var mvcBuilder = services.AddControllersWithViews()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Add("/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/{2}/{0}.cshtml");
                });

            // TODO: Configure AddSessionStateTempDataProvider or AddCookieTempDataProvider

            services.AddRazorPages();

            // TODO: AddMvcOptions

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            // TODO: This is a temporary approach. We should instead find all assemblies and then
            // add validators from them.
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

            // TODO: AddControllersAsServices()

            return mvcBuilder;
        }
    }
}
