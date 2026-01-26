using GlideBuy.Core.Caching;
using GlideBuy.Core.Domain.Media;
using GlideBuy.Models;
using GlideBuy.Models.Catalog;
using GlideBuy.Models.Media;
using GlideBuy.Services.Media;
using GlideBuy.Web.Infrastructure.Cache;

namespace GlideBuy.Web.Factories
{
	public class ProductModelFactory : IProductModelFactory
	{
		private readonly MediaSettings _mediaSettings;
		private readonly IStaticCacheManager _staticCacheManager;
		private readonly IPictureService _pictureService;

		public ProductModelFactory(
			MediaSettings mediaSettings,
			IStaticCacheManager staticCacheManager,
			IPictureService pictureService)
		{
			_mediaSettings = mediaSettings;
			_staticCacheManager = staticCacheManager;
			_pictureService = pictureService;
		}

		private async Task<IList<PictureModel>> PrepareProductOverviewPictureModelsAsync(Product product, int? productThumbPictureSize = null)
		{
			ArgumentNullException.ThrowIfNull(product);

			var pictureSize = productThumbPictureSize ?? _mediaSettings.ProductThumbnailPictureSize;

			var cacheKey = _staticCacheManager.BuildKeyWithDefaultCacheTime(ModelCacheDefaults.ProductOverviewPicturesModelKey, product, pictureSize);

			var cachedPictures = await _staticCacheManager.TryGetOrLoadAsync(cacheKey, async () =>
			{
				async Task<PictureModel> preparePictureModelAsync(Picture picture)
				{
					return new PictureModel
					{

					};
				}

				// TODO: Make the number of pictures to display configurable.
				var pictures = (await _pictureService.GetPicturesByProductAsync(product.Id, 0)).DefaultIfEmpty<Picture?>(null);

				var pictureModels = new List<PictureModel>();
				return pictureModels;
			});

			return cachedPictures;
		}

		public async Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelsAsync(
			IEnumerable<Product> products,
			bool preparePictureModel = true,
			int? productThumbPictrueSize = null)
		{
			ArgumentNullException.ThrowIfNull(products);

			var models = new List<ProductOverviewModel>();

			foreach (var product in products)
			{
				var model = new ProductOverviewModel
				{
					Id = product.Id,
					Name = product.Name, // TODO: Localize name.
					ShortDescription = product.ShortDescription,
					FullDescription = product.LongDescription,
					// TODO: Handle mark as new
				};

				// TODO: Handle price

				if (preparePictureModel)
				{
					model.PictureModels = await PrepareProductOverviewPictureModelsAsync(product, productThumbPictrueSize);
				}

				// TODO: Handle reviews

				models.Add(model);
			}

			return models;
		}
	}
}
