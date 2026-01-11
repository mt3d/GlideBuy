using GlideBuy.Services.Catalog;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Controllers
{
	public class CatalogController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly ICatalogModelFactory _catalogModelFactory;

		public int PageSize = 4;

		public CatalogController(
			ICategoryService categoryService,
			ICatalogModelFactory categoryModelFactory)
		{
			_categoryService = categoryService;
			_catalogModelFactory = categoryModelFactory;
		}

		public async Task<IActionResult> Category(int categoryId)
		{
			var category = await _categoryService.GetCategoryByIdAsync(categoryId);
			if (category is null)
			{
				return NotFound();
			}

			var model = await _catalogModelFactory.PrepareCategoryModelAsync(category);

			return View(model);
		}
	}
}
