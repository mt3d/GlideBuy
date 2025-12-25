using GlideBuy.Models;
using GlideBuy.Models.Catalog;

namespace GlideBuy.Web.Factories
{
	public interface IProductModelFactory
	{
		Task<IEnumerable<ProductOverviewModel>> PrepareProductOverviewModelsAsync(
			IEnumerable<Product> products);
	}
}
