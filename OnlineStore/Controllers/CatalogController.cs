using GlideBuy.Data.Repositories;
using GlideBuy.Services.Catalog;
using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Controllers
{
	public class CatalogController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly ICatalogModelFactory _catalogModelFactory;

		private readonly ProductRepository productRepository;

		public int PageSize = 4;

		public CatalogController(
			ProductRepository productRepository,
			ICategoryService categoryService,
			ICatalogModelFactory categoryModelFactory)
		{
			this.productRepository = productRepository;
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
