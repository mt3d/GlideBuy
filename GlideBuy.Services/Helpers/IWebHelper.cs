namespace GlideBuy.Services.Helpers
{
    public interface IWebHelper
    {
        string GetStoreLocation(bool? useSsl = null);
    }
}
