using GlideBuy.Core;
using GlideBuy.Core.Domain.Catalog;
using GlideBuy.Core.Domain.Media;
using GlideBuy.Core.Infrastructure;
using GlideBuy.Data;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace GlideBuy.Services.Media
{
    public class PictureService : IPictureService
    {
        protected readonly IDataRepository<Picture> _pictureRepository;
        protected readonly IDataRepository<ProductPicture> _productPictureRepository;
        protected readonly IDataRepository<PictureBinary> _pictureBinaryRepository;
        protected readonly IGlideBuyFileProvider _fileProvider;
        protected readonly MediaSettings _mediaSettings;
        protected readonly IThumbService _thumbService;

        public PictureService(
            IDataRepository<Picture> pictureRepository,
            IDataRepository<ProductPicture> productPictureRepository,
            IGlideBuyFileProvider fileProvider,
            MediaSettings mediaSettings,
            IThumbService thumbService,
            IDataRepository<PictureBinary> pictureBinaryRepository)
        {
            _pictureRepository = pictureRepository;
            _productPictureRepository = productPictureRepository;
            _fileProvider = fileProvider;
            _mediaSettings = mediaSettings;
            _thumbService = thumbService;
            _pictureBinaryRepository = pictureBinaryRepository;
        }

        #region Utilities

        // Called by LoadPictureBinaryAsync()
        protected virtual async Task<byte[]> LoadPictureFromFileAsync(int pictureId, string mimeType)
        {
            /**
             * Conventions:
             * File naming format: {pictureId}_0.{extension}
             * Extension is derived from MIME type (not stored separately)
             */
            var extension = GetFileExtensionFromMimeTypeAsync(mimeType);
            var fileName = $"{pictureId:0000000}_0.{extension}";
            var filePath = await GetPictureLocalPathAsync(fileName);

            return await _fileProvider.ReadAllBytesAsync(filePath);
        }

        protected virtual Task<string> GetPictureLocalPathAsync(string filename)
        {
            return Task.FromResult(_fileProvider.Combine(_fileProvider.GetLocalImagesPath(_mediaSettings), filename));
        }

        protected virtual async Task<byte[]> LoadPictureBinaryAsync(Picture picture, bool fromDb)
        {
            ArgumentNullException.ThrowIfNull(picture);

            var result = fromDb
                ? (await GetPictureBinaryByPictureIdAsync(picture.Id))?.BinaryData ?? Array.Empty<byte>()
                : await LoadPictureFromFileAsync(picture.Id, picture.MimeType);

            return result;
        }

        /**
         * SKEncodedImageFormat Enum: The various formats used by a SKCodec.
         */
        protected virtual SKEncodedImageFormat GetImageFormatByMimeType(string mimeType)
        {
            var format = SKEncodedImageFormat.Jpeg;
            if (string.IsNullOrEmpty(mimeType))
                return format;

            var components = mimeType.ToLowerInvariant().Split('/');
            var subtype = components[1];

            switch (subtype)
            {
                case "webp":
                    format = SKEncodedImageFormat.Webp;
                    break;
                case "png":
                case "gif":
                case "bmp":
                case "x-icon":
                    format = SKEncodedImageFormat.Png;
                    break;
                default:
                    break;
            }

            return format;
        }

        /**
         * In SkiaSharp (and most UI frameworks), the image lives in a grid like this:
         * (0,0) ---------> x
         * |
         * |
         * |
         * v
         * y
         * 
         * Top-left corner = (0, 0)
         * x increases to the right
         * y increases downward (this is important!)
         * 
         * bitmap (width = 100, height = 50)
         * top-left: (0,0)
         * top-right: (100,0)
         * bottom-left: (0,50)
         * bottom-right: (100,50)
         * 
         * The origin is simply the point (0,0). Transformations (rotation, scaling)
         * happen around the origin.
         * If you rotate the whole canvas the point moves relative to (0,0).
         * But here’s the problem: If you rotate an image without moving it first, part of
         * it will go into negative coordinates (off-screen).
         * 
         * 
         * When images are taken on phones or cameras, they are often:
         * Stored unrotated
         * With an EXIF orientation flag that tells viewers how to display them
         * 
         * So instead of rotating pixels, the file says this image should be rotated
         * 90° clockwise when displayed. Browsers respect this. Raw image processing
         * libraries usually do not. So without this method thumbnails would appear
         * sideways or upside down
         * 
         * SKEncodedOrigin: Represents various origin values returned by Origin.
         */
        protected virtual SKBitmap AutoOrient(SKBitmap bitmap, SKEncodedOrigin origin)
        {
            SKBitmap rotated;

            /**
             * An SKCanvas is best understood as a drawing context. It is not the image
             * itself. Instead, it is the tool you use to draw onto an image.
             * 
             * SKBitmap = the pixel buffer (the actual image in memory)
             * SKCanvas = a drawing API bound to that bitmap
             * 
             * You cannot "rotate" or “draw” directly on raw pixel memory in a convenient way.
             */

            switch (origin)
            {
                case SKEncodedOrigin.BottomRight: // 180
                    using (var surface = new SKCanvas(bitmap)) // "This is the surface I am drawing on"
                    {
                        // Reuse the same bitmap
                        // 180° rotation does not change width/height
                        // Rotate around center, not origin.
                        surface.RotateDegrees(180, bitmap.Width / 2f, bitmap.Height / 2f);

                        /** "Take this bitmap and paint it onto the canvas at position (0,0)."
                        // The canvas is transformed
                         * Then the bitmap is drawn into that transformed coordinate system
                         * We are drawing a copy of the bitmap onto itself. Why? Because otherwise
                         * you would be reading pixels from the bitmap while simultaneously writing into it.
                        */
                        surface.DrawBitmap(bitmap.Copy(), 0, 0);
                    }
                    return bitmap;

                // In the following cases, we are not rotating the images.
                // Instead we are rotating the coordinates first, then we are drawing
                // the image using those transformed coordinates.

                case SKEncodedOrigin.RightTop: // 90
                    // Swap width and height
                    // A 90° or 270° rotation flips dimensions (the x becomes y, and y becomes x)
                    // Create a new bitmap whenever the transformation changes the image dimensions.
                    rotated = new SKBitmap(bitmap.Height, bitmap.Width);
                    using (var surface = new SKCanvas(rotated))
                    {
                        /**
                         * Move origin to the right edge
                         * Rotate coordinate system clockwise 90°
                         * Draw original bitmap into this transformed space
                         */
                        surface.Translate(rotated.Width, 0);
                        surface.RotateDegrees(90);
                        surface.DrawBitmap(bitmap, 0, 0);
                    }
                    return rotated;
                case SKEncodedOrigin.LeftBottom: // 270
                    rotated = new SKBitmap(bitmap.Height, bitmap.Width);
                    using (var surface = new SKCanvas(rotated))
                    {
                        surface.Translate(0, rotated.Height);
                        surface.RotateDegrees(270);
                        surface.DrawBitmap(bitmap, 0, 0);
                    }
                    return rotated;
                default:
                    return bitmap;
            }
        }

        // Think of targetSize as a desired maximum dimension.
        protected virtual byte[] ImageResize(SKBitmap imageBitmap, SKEncodedImageFormat format, int targetSize, SKEncodedOrigin? encodedOrigin = null)
        {
            ArgumentNullException.ThrowIfNull(imageBitmap);

            if (encodedOrigin is not null)
                imageBitmap = AutoOrient(imageBitmap, encodedOrigin.Value);

            float width, height, aspectRatio;

            // computes the new dimensions while preserving the aspect ratio
            // The longest dimension should become targetSize.
            // Aspect ration = short/long
            // The longest side of the image becomes targetSize, while the other side scales proportionally.
            if (imageBitmap.Height > imageBitmap.Width)
            {
                // portrait
                aspectRatio = imageBitmap.Width / (float)imageBitmap.Height;
                height = targetSize;
                width = aspectRatio * height;
            }
            else
            {
                aspectRatio = imageBitmap.Height / (float)imageBitmap.Width;
                width = targetSize;
                height = aspectRatio * width;
            }

            /**
             * This protects against edge cases where extremely small images or rounding errors could produce zero dimensions, which would otherwise cause runtime failures during bitmap creation.
             */
            if ((int)width == 0 || (int)height == 0)
            {
                width = imageBitmap.Width;
                height = imageBitmap.Height;
            }

            try
            {
                /**
                 * The method chooses linear filtering with mipmapping, which is a deliberate quality-performance trade-off. Linear filtering smooths the image during scaling, while mipmaps improve quality when reducing image size significantly, preventing aliasing artifacts. This reflects a production-oriented decision to favor visually acceptable thumbnails without incurring excessive computational cost.
                 */
                var samplingOption = new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear);
                using var resizedBitmap = imageBitmap.Resize(new SKImageInfo((int)width, (int)height), samplingOption);

                /**
                 * This step transitions from raw pixel data (SKBitmap) to a compressed format (JPEG, PNG, etc.), using the provided SKEncodedImageFormat and a configurable quality setting.
                 * 
                 * The quality parameter is sourced from _mediaSettings.DefaultImageQuality, with a fallback to 80, which is a commonly accepted balance between file size and visual fidelity.
                 */
                using var cropImage = SKImage.FromBitmap(resizedBitmap);
                return cropImage.Encode(format, _mediaSettings.DefaultImageQuality > 0 ? _mediaSettings.DefaultImageQuality : 80).ToArray();
            }
            catch
            {
                /**
                 * This is a resilience mechanism. If resizing fails for any reason, such
                 * as unsupported formats or decoding issues, the method falls back to
                 * returning the original image bytes.
                 */
                return imageBitmap.Bytes;
            }
        }

        protected virtual async Task<PictureBinary> UpdatePictureBinaryAsync(Picture picture, byte[] binaryData)
        {
            ArgumentNullException.ThrowIfNull(picture);

            var pictureBinary = await GetPictureBinaryByPictureIdAsync(picture.Id);

            var isNew = pictureBinary == null;

            if (isNew)
            {
                pictureBinary = new PictureBinary
                {
                    PictureId = picture.Id
                };
            }

            pictureBinary!.BinaryData = binaryData;

            if (isNew)
                await _pictureBinaryRepository.InsertAsync(pictureBinary);
            else
                await _pictureBinaryRepository.UpdateAsync(pictureBinary);

            return pictureBinary;
        }

        protected virtual async Task SavePictureInFileAsync(int pictureId, byte[] pictureBinary, string mimeType)
        {
            var lastPart = GetFileExtensionFromMimeTypeAsync(mimeType);
            var fileName = $"{pictureId:000000}_0.{lastPart}";
            await _fileProvider.WriteAllBytesAsync(await GetPictureLocalPathAsync(fileName), pictureBinary);
        }

        #endregion

        public async Task<IList<Picture>> GetPicturesByProductAsync(int productId, int recordsToReturn = 0)
        {
            if (productId == 0)
            {
                return new List<Picture>();
            }

            // TODO: Convert to fluent.
            var query = from p in _pictureRepository.Table
                        join pp in _productPictureRepository.Table on p.Id equals pp.PictureId
                        orderby pp.DisplayOrder, pp.Id
                        where pp.ProductId == productId
                        select p;

            if (recordsToReturn > 0)
            {
                query = query.Take(recordsToReturn);
            }

            var pics = await query.ToListAsync();
            return pics;
        }

        /**
         * GetPictureUrlAsync has two responsibilities that are intentionally intertwined.
         * 
         * The first responsibility is logical resolution: deciding which image should be served, whether it is a real picture, a resized thumbnail, or a fallback default image.
         * 
         * The second responsibility is physical realization: ensuring that the required thumbnail file actually exists on disk before returning its URL.
         * 
         * This means the method is not a pure query, but a lazy materialization routine. Asking for a picture URL can trigger database updates, image decoding, resizing, filesystem writes, and synchronization across threads.
         * 
         * GetPictureUrlAsync is not primarily an image-processing method; it is a thumbnail cache manager.
         */
        public async Task<(string Url, Picture? picture)> GetPictureUrlAsync(
            Picture picture,
            int targetSize = 0, // targetSize = 0 means no resize
            bool showDefaultPicture = true,
            string? storeLocation = null)
            // TODO: Use PictureType
        {
            /**
             * Null picture reference is not treated as an error state, but as a normal
             * scenario, because Nop allows entities to exist without images.
             */
            if (picture is null)
            {
                // TODO: Add the ability to return a default picture.
                return (string.Empty, null);
            }

            byte[]? pictureBinary = null;

            /**
             * IsNew == true means: "this image has not yet been normalized and
             * integrated into the thumbnail system."
             * This can happen in several scenarios:
             * Image just uploaded
             * Image migrated (e.g., DB ↔ file system)
             * Image updated (SEO filename, binary, etc.)
             * 
             * This whole block is a lazy initialization mechanism.
             * "Process and normalize the image the first time it is actually needed."
             * 
             * This reduces upfront cost and unnecessary processing.
             */
            if (picture.IsNew)
            {
                // Step 1: Invalidate cache
                // All thumbnails are deleted because:
                // They may be outdated
                // Naming may change (SEO filename affects thumb name)
                await _thumbService.DeletePictureThumbsAsync(picture);

                // Step 2: Load raw data
                pictureBinary = await LoadPictureBinaryAsync(picture);

                // TODO: Show default picture if the binary is empty.

                // Step 4: Normalize and persist

                // TODO: Update the picture from the binary. Why?
                picture = (await UpdatePictureAsync(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false,
                    false))!;
            }

            var seoFileName = picture.SeoFilename;
            var extension = await GetFileExtensionFromMimeTypeAsync(picture.MimeType);
            var thumbFileName = !string.IsNullOrWhiteSpace(seoFileName)
                ? $"{picture.Id:0000000}_{seoFileName}.{extension}"
                : $"{picture.Id:0000000}.{extension}";

            if (targetSize == 0 || picture.MimeType == MimeTypes.ImageSvg) // no resize
            {
                // 1. Try to find a thumb with the specified name and return its URL.

                // This just returns the expected path of a thumb file with the specified name.
                var thumbFilePath = await _thumbService.GetThumbLocalPathByFileNameAsync(thumbFileName);

                // This checkes for actual existence.
                if (await _thumbService.GeneratedThumbExistsAsync(thumbFilePath, thumbFileName))
                    return (await _thumbService.GetThumbUrlAsync(thumbFileName, storeLocation), picture);

                // 2. If not found, create a new one from the binary data.

                // Avoids duplicate loading if alread done in IsNew branch.
                pictureBinary ??= await LoadPictureBinaryAsync(picture);

                /**
                 * Imagine this scenario:
                 * 50 users request the same image at the same time
                 * Thumbnail does not exist yet
                 * 
                 * Without synchronization all 50 requests will:
                 * Load the image
                 * Generate the same thumbnail
                 * Try to write to the same file
                 * 
                 * This leads to:
                 * File corruption: Partial writes overlapping
                 * IO exceptions: File locked / already exists / inconsistent state
                 * Race conditions: Two threads writing the same file simultaneously
                 * Massive unnecessary CPU work: Multiple expensive resizes for same image
                 * 
                 * Only one thread (across the entire system) can generate this specific thumbnail at a time.
                 * 
                 * A lock (monitor) is:
                 * In-process only
                 * Works only inside the same application instance
                 * 
                 * A named mutex:
                 * Works across threads AND processes
                 * Identified by name (thumbFileName)
                 * 
                 * Why not SemaphoreSlim? It throws PlatformNotSupportedException on UNIX
                 * 
                 * You cannot safely await inside a mutex. The thread may resume on a
                 * different context. Mutex ownership would break
                 */

                // The lock is per file.
                // Each thumbnail has its own lock
                // Different images can be processed in parallel
                using var mutex = new Mutex(false, thumbFileName);
                mutex.WaitOne();
                try
                {
                    _thumbService.SaveThumbAsync(thumbFilePath, thumbFileName, picture.MimeType, pictureBinary).Wait();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else // resize
            {
            }

            return (await _thumbService.GetThumbUrlAsync(thumbFileName, storeLocation), picture);
        }

        /**
         * No internal dependencies.
         * 
         * A MIME type most commonly consists of just two parts: a type and a subtype,
         * separated by a slash (/) — with no whitespace between:
         * type/subtype
         * 
         * The type represents the general category into which the data type falls, such
         * as video or text.
         * The subtype identifies the exact kind of data of the specified type the MIME
         * type represents. For example, for the MIME type text, the subtype might be
         * plain (plain text), html (HTML source code), or calendar (for iCalendar/.ics) files.
         */
        public Task<string?> GetFileExtensionFromMimeTypeAsync(string mimeType)
        {
            if (mimeType is null)
            {
                return Task.FromResult<string?>(null);
            }

            var parts = mimeType.Split('/');
            var lastPart = parts[^1];

            lastPart = lastPart switch
            {
                "pjpeg" => "jpg",
                "jpeg" => "jpeg",
                "x-png" or "png" => "png",
                "webp" => "webp",
                _ => "",
            };

            return Task.FromResult<string?>(lastPart);
        }

        public virtual async Task<byte[]> LoadPictureBinaryAsync(Picture picture)
        {
            return await LoadPictureBinaryAsync(picture, await IsStoreInDbAsync());
        }

        public virtual async Task<bool> IsStoreInDbAsync()
        {
            return true;

            throw new NotImplementedException();
        }

        public virtual async Task<PictureBinary?> GetPictureBinaryByPictureIdAsync(int pictureId)
        {
            return await _pictureBinaryRepository.Table.FirstOrDefaultAsync(pb => pb.PictureId == pictureId);
        }

        /**
         * Effectively a synchronization point between metadata, binary storage, and the thumbnail cache
         */
        public virtual async Task<Picture?> UpdatePictureAsync(
            int pictureId,
            byte[] pictureBinary,
            string mimeType,
            string seoFilename,
            string? altAttribute = null,
            string? titleAttribute = null,
            bool isNew = true,
            bool validateBinary = true)
        {
            // Services enforce invariants before touching persistence.
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);
            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            // TODO: Validate the binary data
            if (validateBinary)
                throw new NotImplementedException();

            var picture = await GetPictureByIdAsync(pictureId);
            if (picture is null)
                return null;

            // Delete the old thumbs if the name has changed, because the thumbnail
            // filename depends on the SEO filename.
            if (seoFilename != picture.SeoFilename)
                await _thumbService.DeletePictureThumbsAsync(picture);

            // Metadata update

            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;
            await _pictureRepository.UpdateAsync(picture);

            // The database is always kept in a consistent state, even when not used for storage.
            await UpdatePictureBinaryAsync(picture, await IsStoreInDbAsync() ? pictureBinary : Array.Empty<Byte>());

            // TODO: Save the file is we're storing pictures in files.
            if (!await IsStoreInDbAsync())
                await SavePictureInFileAsync(picture.Id, pictureBinary, mimeType);

            return picture;
        }

        public virtual async Task<Picture?> GetPictureByIdAsync(int pictureId)
        {
            // A lambda that returns the default value => use the default cache key.
            return await _pictureRepository.GetByIdAsync(pictureId, cache => default);
        }
    }
}
