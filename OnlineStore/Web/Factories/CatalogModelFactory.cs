using GlideBuy.Core.Caching;
using GlideBuy.Data;
using GlideBuy.Web.Infrastructure.Cache;
using GlideBuy.Web.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Web.Factories
{
	public class CatalogModelFactory : ICatalogModelFactory
	{
		private readonly IStaticCacheManager _staticCacheManager;
		private readonly StoreDbContext _context;

		public CatalogModelFactory(
			IStaticCacheManager staticCacheManager,
			StoreDbContext storeDbContext)
		{
			_staticCacheManager = staticCacheManager;
			_context = storeDbContext;
		}

		public async Task<List<CategoryModel>> PrepareHomePageCategoryModelsAsync()
		{
			// TODO: Check the following: store Id, user role, language, picture size, secure connection.
			var categoriesCacheKey = _staticCacheManager.BuildKeyWithDefaultCacheTime(
				ModelCacheDefaults.CategoryHomepageKey);

			var model = await _staticCacheManager.TryGetOrLoad(categoriesCacheKey, async () =>
			{
				// TODO: Use categories service
				var categories = await _context.Categories.ToListAsync();

				return categories.Select(category =>
				{
					var categoryModel = new CategoryModel
					{
						Id = category.Id,
						Name = category.Name,

						// TODO: Use a service to generate a search-engine friendly name
						SearchEngineName = category.Name
					};

					return categoryModel;
				}).ToList();
			});

			return model;
		}
	}
}
