using GlideBuy.Web.Models.Catalog;

namespace GlideBuy.Web.Factories
{
	public interface ICatalogModelFactory
	{
		Task<List<CategoryModel>> PrepareHomePageCategoryModelsAsync();
	}
}
