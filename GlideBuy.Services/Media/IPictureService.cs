using GlideBuy.Core.Domain.Media;

namespace GlideBuy.Services.Media
{
    public interface IPictureService
    {
        Task<IList<Picture>> GetPicturesByProductAsync(int productId, int recordsToReturn = 0);

        Task<(string Url, Picture? picture)> GetPictureUrlAsync(
            Picture picture,
            int targetSize = 0, // targetSize = 0 means no resize
            bool showDefaultPicture = true,
            string? storeLocation = null);

        Task<string> GetPictureUrlAsync(
            int pictureId,
            int targetSize,
            bool showDefaultPicture = true,
            string? storeLocation = null);

        Task<string?> GetFileExtensionFromMimeTypeAsync(string mimeType);

        Task<byte[]> LoadPictureBinaryAsync(Picture picture);

        Task<bool> IsStoreInDbAsync();

        Task<PictureBinary?> GetPictureBinaryByPictureIdAsync(int pictureId);

        Task<Picture?> UpdatePictureAsync(
            int pictureId,
            byte[] pictureBinary,
            string mimeType,
            string seoFilename,
            string? altAttribute = null,
            string? titleAttribute = null,
            bool isNew = true,
            bool validateBinary = true);

        Task<Picture?> GetPictureByIdAsync(int pictureId);
    }
}
