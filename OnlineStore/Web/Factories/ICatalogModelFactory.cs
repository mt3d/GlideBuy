using GlideBuy.Models;
using GlideBuy.Models.Catalog;
using GlideBuy.Web.Models.Catalog;

namespace GlideBuy.Web.Factories
{
	public interface ICatalogModelFactory
	{
		Task<List<CategoryModel>> PrepareHomePageCategoryModelsAsync();

		Task<CategoryNavigationModel> PrepareCategoriesMegaMenuModelAsync();

		Task<CategoryModel> PrepareCategoryModelAsync(Category category);

		Task<CatalogProductsModel> PrepareCategoryProductsModelAsync(Category category);
	}
}
