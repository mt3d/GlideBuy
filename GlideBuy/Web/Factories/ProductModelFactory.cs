using GlideBuy.Models;
using GlideBuy.Models.Catalog;
using GlideBuy.Models.Media;

namespace GlideBuy.Web.Factories
{
	public class ProductModelFactory : IProductModelFactory
	{
		private async Task<IList<PictureModel>> PrepareProductOverviewPictureModelsAsync(Product product, int? productThumbPictureSize = null)
		{
			ArgumentNullException.ThrowIfNull(product);

			var cachedPictures = new List<PictureModel>();

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
				
				// TODO: Handle picture

				// TODO: Handle reviews

				models.Add(model);
			}

			return models;
		}
	}
}
