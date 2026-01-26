using GlideBuy.Core.Domain.Media;
using GlideBuy.Data;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Services.Media
{
	public class PictureService : IPictureService
	{
		private readonly IDataRepository<Picture> _pictureRepository;
		private readonly IDataRepository<ProductPicture> _productPictureRepository;

		public PictureService(
			IDataRepository<Picture> pictureRepository,
			IDataRepository<ProductPicture> productPictureRepository)
		{
			_pictureRepository = pictureRepository;
			_productPictureRepository = productPictureRepository;
		}

		private async Task<byte[]> LoadPictureFromFileAsync(int pictureId, string mimeType)
		{
			var extension = GetFileExtensionFromMimeTypeAsync(mimeType);
			var fileName = $"{pictureId:0000000}_0.{extension}";

			throw new NotImplementedException();
		}

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
		 */
		public async Task<(string Url, Picture? picture)> GetPictureUrlAsync(Picture picture, int targetSize = 0)
		{
			// TODO: Add the ability to return a default picture.
			/**
			 * The method begins with defensive handling of a null Picture reference. This is not treated as an error state, but as a normal scenario, because Nop allows entities to exist without images.
			 */
			if (picture is null)
			{
				return (string.Empty, null);
			}

			byte[] pictureBinary = null;
			if (picture.IsNew)
			{

			}

			var seoFileName = picture.SeoFilename;
			var extension = await GetFileExtensionFromMimeTypeAsync(picture.MimeType);
			var thumbFileName = !string.IsNullOrWhiteSpace(seoFileName)
				? $"{picture.Id:0000000}_{seoFileName}.{extension}"
				: $"{picture.Id:0000000}.{extension}";

			throw new NotImplementedException();
		}

		/**
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
	}
}
