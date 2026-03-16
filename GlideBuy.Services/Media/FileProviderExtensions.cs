using GlideBuy.Core.Domain.Media;
using GlideBuy.Core.Infrastructure;

namespace GlideBuy.Services.Media
{
    public static class FileProviderExtensions
    {
        /*
         * Get absolute path of local images.
         * 
         * Conceptually, the purpose of GetLocalImagesPath is to convert a logical image path from configuration into a fully qualified absolute file system path that the application can safely use.
         * 
         * Whenever ThumbService calls
         * FileProvider.GetLocalImagesPath(_mediaSettings)
         * it is obtaining the absolute root directory for all media images.
         * From there it appends images/thumbs/ (via NopMediaDefaults.ImageThumbsPath)
         * to reach the thumbnail storage directory.
         */
        public static string GetLocalImagesPath(this IGlideBuyFileProvider fileProvider, MediaSettings mediaSettings, string? path = null)
        {
            /**
             * If the optional path parameter is not supplied, the method reads
             * the value from MediaSettings.PicturePath. This setting allows the
             * administrator to configure a custom directory where images are stored.
             * If the configuration value is empty, the system falls back to NopMediaDefaults.DefaultImagesPath. That constant represents the standard images folder used by NopCommerce installations.
             */
            if (string.IsNullOrEmpty(path))
            {
                var imagePath = mediaSettings.PicturePath;

                path = string.IsNullOrEmpty(imagePath) ? GlideBuyMediaDefaults.DefaultImagesPath : imagePath;
            }

            /**
             * At this stage, the variable path represents a configured logical location, but it might not yet be a valid absolute path on the operating system.
             * 
             * The second step checks whether the path is already rooted. A rooted path means that it already starts from the file system root.
             */
            if (!fileProvider.IsPathRooted(path))
                path = fileProvider.GetAbsolutePath(path);

            return path;
        }
    }
}
