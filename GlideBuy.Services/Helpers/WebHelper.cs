using Microsoft.AspNetCore.Http;

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
            throw new NotImplementedException();
        }

        public virtual bool IsCurrentConnectionSecured()
        {
            throw new NotImplementedException();
        }

        /**
         * Determines the base URL of the application.
         */
        public string GetStoreLocation(bool? useSsl = null)
        {
            var storeLocation = string.Empty;



            return storeLocation;
        }
    }
}
