using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace GlideBuy.Services.Helpers
{
    /**
     * Safely derives runtime information from the current HTTP request without
     * breaking in non-request scenarios such as background jobs.
     */
    public class WebHelper : IWebHelper
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public WebHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext is null)
                return false;

            /**
             * The try-catch block may look redundant, but it exists to guard against edge
             * cases where accessing HttpContext.Request might throw, for example due to
             * disposal timing or unusual hosting scenarios. Instead of letting such exceptions
             * propagate, the method simply returns false. In effect, this method answers the
             * question: “Is it safe to read request data right now?”
             */
            try
            {
                if (_httpContextAccessor.HttpContext.Request is null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public virtual string GetStoreHost(bool useSsl)
        {
            if (!IsRequestAvailable())
                return string.Empty;

            /**
             * This header typically contains values such as example.com or example.com:5000. If the header is missing or empty, the method again returns an empty string, signaling that it cannot determine the host dynamically.
             * 
             * One subtle but important design decision here is the reliance on the Host header rather than hardcoded configuration. This makes the system adaptable to environments like reverse proxies, load balancers, and containerized deployments, where the externally visible host may differ from internal configuration.
             */
            var hostHeader = _httpContextAccessor.HttpContext!.Request.Headers[HeaderNames.Host];
            if (StringValues.IsNullOrEmpty(hostHeader))
                return string.Empty;

            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

            /**
             * The method then normalizes the result by ensuring it ends with a trailing slash. This mirrors the behavior you saw earlier and ensures consistent concatenation when building full URLs.
             */
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        public virtual bool IsCurrentConnectionSecured()
        {
            /**
             * This is a design choice: in the absence of a request, the system assumes a non-secure context.
             */
            if (!IsRequestAvailable())
                return false;

            return _httpContextAccessor.HttpContext!.Request.IsHttps;
        }

        /**
         * Determines the base URL of the application.
         */
        public string GetStoreLocation(bool? useSsl = null)
        {
            var storeLocation = string.Empty;

            var storeHost = GetStoreHost(useSsl ?? IsCurrentConnectionSecured());
            if (!string.IsNullOrEmpty(storeHost))
            {
                /**
                 * This ensures correctness when the application is deployed under a sub-path.
                 */
                storeLocation = IsRequestAvailable()
                    ? $"{storeHost.TrimEnd('/')}{_httpContextAccessor.HttpContext!.Request.PathBase}"
                    : storeHost;
            }

            /**
             * This can happen when HttpContext is not available at all, for example in background tasks, scheduled jobs, or certain initialization phases./
             */
            if (string.IsNullOrEmpty(storeHost))
            {
                // TODO: Try to determine the configured store URL from the database.
                throw new NotImplementedException();
            }

            storeLocation = $"{storeLocation.TrimEnd('/')}/";

            return storeLocation;
        }
    }
}
