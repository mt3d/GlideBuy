using GlideBuy.Models;
using GlideBuy.Models.Catalog;

namespace GlideBuy.Web.Factories
{
	public class ProductModelFactory : IProductModelFactory
	{
		public async Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelsAsync(
			IEnumerable<Product> products)
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
				
				// TODO: Handle picture

				// TODO: Handle reviews

				models.Add(model);
			}

			return models;
		}
	}
}
