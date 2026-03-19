namespace GlideBuy.Services.Helpers
{
    public interface IWebHelper
    {
        bool IsCurrentConnectionSecured();

        string GetStoreHost(bool useSsl);

        string GetStoreLocation(bool? useSsl = null);
    }
}
