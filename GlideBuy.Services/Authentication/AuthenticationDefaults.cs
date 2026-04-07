using Microsoft.AspNetCore.Http;

namespace GlideBuy.Services.Authentication
{
    public class AuthenticationDefaults
    {
        public static string AuthenticationScheme => "GlideBuyAuthentication";

        public static PathString LoginPath => new("/login");
    }
}
