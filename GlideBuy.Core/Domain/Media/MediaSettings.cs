using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Domain.Media
{
    public class MediaSettings : ISettings
    {
        public int ProductThumbnailPictureSize { get; set; }

        public string PicturePath { get; set; }

        /**
         * Single: thumbs/
         * Multiple: thumbs/001, thumbs/002
         */
        public bool MultipleThumbDirectories { get; set; }

        public bool UseAbsoluteImagePath { get; set; }
    }
}
