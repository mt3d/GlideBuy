using GlideBuy.Core.Domain.Media;
using GlideBuy.Core.Infrastructure;

namespace GlideBuy.Services.Media
{
    /**
     * ThumbService is intentionally very small because it is not responsible
     * for image processing or thumbnail generation logic. Instead, it is a
     * thin infrastructure service whose job is limited to file management
     * and URL construction. The real orchestration and decision-making lives
     * inside PictureService, especially in GetPictureUrlAsync.
     * 
     * PictureService decides whether a thumbnail should exist and how it should be generated. It loads the original image, determines the required size, performs resizing, and controls concurrency with the mutex.
     * 
     * ThumbService decides where the thumbnail is stored and how it is accessed. It provides the file path, saves the binary, checks for existence, and produces the public URL.
     * 
     * This separation of responsibilities explains why GetPictureUrlAsync appears to be so large compared with ThumbService. The former implements the entire thumbnail lifecycle, while the latter provides only the storage abstraction that supports that lifecycle.
     */
    public class ThumbService : IThumbService
    {
        protected readonly IGlideBuyFileProvider _fileProvider;
        protected readonly MediaSettings _mediaSettings;

        public ThumbService(
            IGlideBuyFileProvider fileProvider,
            MediaSettings mediaSettings)
        {
            _fileProvider = fileProvider;
            _mediaSettings = mediaSettings;
        }

        // The method does not perform resizing, validation, or any transformation;
        // it simply persists the binary that it receives.
        // TODO: thumbFileName and mimeType are not used.
        public virtual async Task SaveThumbAsync(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            var thumbsDirectoryPath = _fileProvider.Combine(_fileProvider.GetLocalImagesPath(_mediaSettings), GlideBuyMediaDefaults.ImageThumbsPath);
            _fileProvider.CreateDirectory(thumbsDirectoryPath);

            await _fileProvider.WriteAllBytesAsync(thumbFilePath, binary);
        }
    }
}
