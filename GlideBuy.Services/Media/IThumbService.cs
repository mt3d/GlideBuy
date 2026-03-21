using GlideBuy.Core.Domain.Media;

namespace GlideBuy.Services.Media
{
    public interface IThumbService
    {
        Task SaveThumbAsync(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary);

        Task<string> GetThumbLocalPathByFileNameAsync(string thumbFileName);

        Task<bool> GeneratedThumbExistsAsync(string thumbFilePath, string thumbFileName);

        Task<string> GetThumbUrlAsync(string thumbFileName, string? storeLocation = null);

        Task DeletePictureThumbsAsync(Picture picture);

        Task<string> GetThumbLocalPathAsync(string thumbUrl);
    }
}
