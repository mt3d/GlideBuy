using GlideBuy.Services.Authentication;

namespace GlideBuy.Core.Infrastructure.StartupConfigurations
{
    public class AuthenticationStartupConfiguration : IStartupConfiguration
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // TODO: add data protection services

            var authenticationBuilder = services.AddAuthentication(options =>
            {
                // The DefaultScheme determines which handler is used when the system needs to
                // authenticate a request automatically, meaning when [Authorize] is used
                // without specifying a scheme.

                // The DefaultChallengeScheme defines what happens when an unauthenticated user
                // tries to access a protected resource, typically resulting in a redirect to a login page.

                // The DefaultSignInScheme, however, is more subtle: it specifies which handler
                // is responsible for temporarily storing identities during sign-in flows, especially
                // for external providers such as OAuth.
            });

            authenticationBuilder.AddCookie(AuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.LoginPath = AuthenticationDefaults.LoginPath;
            });

            // TODO: Add external authentication
            /**
             * The second AddCookie call introduces a separate cookie for external authentication. This is not used to represent a fully logged-in user; instead, it acts as a temporary storage mechanism during external login flows. For example, when a user authenticates via Google or another provider, the external provider returns identity information, which is then stored in this external cookie. At this stage, the user is not yet fully authenticated in the application’s own context. Instead, NopCommerce can inspect this temporary identity, decide whether to link it to an existing account or create a new one, and only then issue the main authentication cookie. This separation is crucial because it prevents partially authenticated states from polluting the main authentication system.
             */

            // TODO: Register external authentication plugins
            /**
             * NopCommerce dynamically discovers external authentication providers and allows each one to register its own authentication scheme. This means that providers like Google, Facebook, or others are not hardcoded but instead injected via plugins. Each registrar receives the AuthenticationBuilder, which allows it to call methods such as AddOAuth, AddOpenIdConnect, or custom handlers.
             */
        }

        public void ConfigureApp(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public StartupOrder Order => StartupOrder.Authentication;
    }
}
