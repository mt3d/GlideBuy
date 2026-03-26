using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Media
{
    public class PictureBinary : BaseEntity
    {
        public byte[] BinaryData { get; set; }

        public int PictureId { get; set; }

        public Picture? Picture { get; set; }
    }
}
