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

    }
}
