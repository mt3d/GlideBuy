using GlideBuy.Core.Domain.Media;
using GlideBuy.Core.Infrastructure;
using GlideBuy.Services.Helpers;
using Microsoft.AspNetCore.Http;

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
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IWebHelper _webHelper;

        public ThumbService(
            IGlideBuyFileProvider fileProvider,
            MediaSettings mediaSettings,
            IHttpContextAccessor httpContextAccessor,
            IWebHelper webHelper)
        {
            _fileProvider = fileProvider;
            _mediaSettings = mediaSettings;
            _httpContextAccessor = httpContextAccessor;
            _webHelper = webHelper;
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

        /**
         * This method converts a logical thumbnail filename such as 0001234_product_300.jpg
         * into a concrete storage path inside the media folder
         */
        public virtual Task<string> GetThumbLocalPathByFileNameAsync(string thumbFileName)
        {
            var thumbsDirectoryPath = _fileProvider.Combine(_fileProvider.GetLocalImagesPath(_mediaSettings), GlideBuyMediaDefaults.ImageThumbsPath);

            /**
             * When the setting MultipleThumbDirectories is enabled, it also introduces
             * a subdirectory derived from the first characters of the filename. This
             * mechanism prevents thousands of files from accumulating in a single directory,
             * which would degrade file system performance.
             */
            if (_mediaSettings.MultipleThumbDirectories)
            {
                var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > GlideBuyMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    /**
                     * C# 8.0: A range specifies the start and end of a range. The start of the range is inclusive, but the end of the range is exclusive, meaning the start is included in the range but the end isn't included in the range. The range [0..^0] represents the entire range, just as [0..sequence.Length] represents the entire range.
                     * 
                     * string[] firstPhrase = words[..4]; // contains "first" through "fourth"
                     */
                    var subdirectoryName = fileNameWithoutExtension[..GlideBuyMediaDefaults.MultipleThumbDirectoriesLength];
                    thumbsDirectoryPath += _fileProvider.Combine(_fileProvider.GetLocalImagesPath(_mediaSettings), GlideBuyMediaDefaults.ImageThumbsPath, subdirectoryName);
                    _fileProvider.CreateDirectory(thumbsDirectoryPath);
                }
            }

            var thumbFilePath = _fileProvider.Combine(thumbsDirectoryPath, thumbFileName);
            return Task.FromResult(thumbFilePath);
        }

        public virtual Task<bool> GeneratedThumbExistsAsync(string thumbFilePath, string thumbFileName)
        {
            return Task.FromResult(_fileProvider.FileExists(thumbFilePath));
        }

        /**
         * Convert a thumbnail filename into a public URL that can be returned to the client. This usually involves combining the store location with the thumbnail folder path.
         * 
         * Unlike other parts of the system, it does not deal with files or binaries at all; its entire responsibility is URL composition, but it does so in a way that respects deployment environments, configuration settings, and directory structure.
         * 
         * It is also worth noting that this method does not verify whether the file actually exists. That responsibility lies with GeneratedThumbExistsAsync. This separation keeps concerns clean: one method checks existence, another constructs access paths.
         */
        public virtual Task<string> GetThumbUrlAsync(string thumbFileName, string? storeLocation = null)
        {
            /**
             * This value is important in scenarios where the application is not hosted at the root of a domain. For example, if the application is hosted under /shop, then PathBase will be /shop. If the application is hosted at the root, this value will simply be an empty string. This ensures that generated URLs remain correct regardless of hosting configuration.
             */
            var pathBase = _httpContextAccessor.HttpContext?.Request.PathBase.Value ?? string.Empty;

            /**
             * If UseAbsoluteImagePath is enabled, the method will use the explicitly provided storeLocation as the base URL. This is typically something like https://example.com/ and is useful when images need to be served with absolute URLs, for example in emails or external integrations. If this setting is disabled, the method instead builds a relative URL based on the current request’s PathBase.
             */
            var imagesPathUrl = _mediaSettings.UseAbsoluteImagePath ? storeLocation : $"{pathBase}/";

            /**
             * This ensures that if the previous step produced an empty value, the system falls back to _webHelper.GetStoreLocation(), which dynamically resolves the full store URL at runtime. This makes the method resilient even when storeLocation is not explicitly provided.
             */
            imagesPathUrl = string.IsNullOrEmpty(imagesPathUrl) ? _webHelper.GetStoreLocation() : imagesPathUrl;
            imagesPathUrl += "images/thumbs/";

            if (_mediaSettings.MultipleThumbDirectories)
            {
                var fileNameWithoutExtensions = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
                if (!string.IsNullOrEmpty(fileNameWithoutExtensions) && fileNameWithoutExtensions.Length > GlideBuyMediaDefaults.MultipleThumbDirectoriesLength)
                {
                    var subdirectoryName = fileNameWithoutExtensions[..GlideBuyMediaDefaults.MultipleThumbDirectoriesLength];
                    imagesPathUrl += imagesPathUrl + subdirectoryName + "/";
                }
            }

            imagesPathUrl += thumbFileName;
            return Task.FromResult(imagesPathUrl);
        }

        /**
         * Plays a role in cache invalidation.
         * Remove all thumbnails associated with a specific picture whenever that picture changes.
         */
        public virtual async Task DeletePictureThumbsAsync(Picture picture)
        {
            /**
             * Here, a search pattern is constructed based on the picture ID. The format specifier 0000000 ensures that the ID is padded to 7 digits, which matches the naming convention used in GetPictureUrlAsync. For example, if the picture ID is 123, this becomes 0000123. The *.* means “match any filename that starts with this prefix and has any extension.” Since all thumbnails are generated with filenames that start with the picture ID, this filter effectively targets all thumbnails belonging to that picture, regardless of size or format.
             */
            var filter = $"";

            var currentFiles = _fileProvider.GetFiles(_fileProvider.Combine(_fileProvider.GetLocalImagesPath(_mediaSettings), GlideBuyMediaDefaults.ImageThumbsPath), filter, false);

            foreach (var currentFileName in currentFiles)
            {
                var thumbFilePath = await GetThumbLocalPathByFileNameAsync(currentFileName);
                _fileProvider.DeleteFile(thumbFilePath);
            }
        }
    }
}
