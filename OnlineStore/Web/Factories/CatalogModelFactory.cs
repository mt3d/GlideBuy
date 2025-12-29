using GlideBuy.Core.Caching;
using GlideBuy.Data;
using GlideBuy.Models.Catalog;
using GlideBuy.Services.Seo;
using GlideBuy.Web.Infrastructure.Cache;
using GlideBuy.Web.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GlideBuy.Web.Factories
{
	public class CatalogModelFactory : ICatalogModelFactory
	{
		private readonly IStaticCacheManager _staticCacheManager;
		private readonly StoreDbContext _context;
		private readonly IUrlRecordService _urlRecordService;

		public CatalogModelFactory(
			IStaticCacheManager staticCacheManager,
			StoreDbContext storeDbContext,
			IUrlRecordService urlRecordService)
		{
			_staticCacheManager = staticCacheManager;
			_context = storeDbContext;
			_urlRecordService = urlRecordService;
		}

		// TODO: Remove?
		public async Task<List<CategoryModel>> PrepareHomePageCategoryModelsAsync()
		{
			// TODO: Check the following: store Id, user role, language, picture size, secure connection.
			var categoriesCacheKey = _staticCacheManager.BuildKeyWithDefaultCacheTime(
				ModelCacheDefaults.CategoryHomepageKey);

			var model = await _staticCacheManager.TryGetOrLoad(categoriesCacheKey, async () =>
			{
				// TODO: Use categories service
				var categories = await _context.Categories.ToListAsync();

				// TODO: Use the .NET 10 AsynEnumerable.
				return await categories.ToAsyncEnumerable().SelectAwait(async category =>
				{
					var categoryModel = new CategoryModel
					{
						Id = category.Id,
						Name = category.Name,

						// TODO: Use a service to generate a search-engine friendly name
						SeName = await _urlRecordService.GetSeNameAsync(category)
					};

					return categoryModel;
				}).ToListAsync();
			});

			return model;
		}

		public async Task<CategoryNavigationModel> PrepareCategoriesMegaMenuModelAsync()
		{
			var cachedCategoriesModel = await PrepareCategorySimpleModelsAsync();

			var model = new CategoryNavigationModel
			{
				Categories = cachedCategoriesModel,
			};

			return model;
		}

		public async Task<List<CategorySimpleModel>> PrepareCategorySimpleModelsAsync()
		{
			// Load and cache the categories.
			var cacheKey = _staticCacheManager.BuildKeyWithDefaultCacheTime(new ("GlideBuy.presentation.category.all"));

			return await _staticCacheManager.TryGetOrLoad(cacheKey, async () => await PrepareCategorySimpleModelsAsync(0));
		}

		public async Task<List<CategorySimpleModel>> PrepareCategorySimpleModelsAsync(int rootCategoryId, bool loadSubcategories = true)
		{
			var result = new List<CategorySimpleModel>();

			// TODO: Use CategoryService
			var allCategories = await _context.Categories.ToListAsync();

			// TODO: Implement display order.
			var categories = allCategories.Where(c => c.ParentCategoryId == rootCategoryId).OrderBy(c => c.DisplayOrder).ToList();

			foreach (var category in categories)
			{
				var categoryModel = new CategorySimpleModel
				{
					Id = category.Id,
					Name = category.Name,
					SeName = await _urlRecordService.GetSeNameAsync(category)
				};

				if (loadSubcategories)
				{
					var subCategories = await PrepareCategorySimpleModelsAsync(category.Id);
					categoryModel.Subcategories.AddRange(subCategories);
				}

				// TODO: Handle have subcategories.

				result.Add(categoryModel);
			}

			return result;
		}
	}
}
