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
	}
}
