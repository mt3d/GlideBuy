using GlideBuy.Data.Repositories;
using GlideBuy.Models.ViewModels;
using GlideBuy.Services.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Controllers
{
	public class CatalogController : Controller
	{
		private readonly ICategoryService _categoryService;

		private readonly ProductRepository productRepository;

		public int PageSize = 4;

		public CatalogController(
			ProductRepository productRepository,
			ICategoryService categoryService)
		{
			this.productRepository = productRepository;
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Category(int categoryId)
		{
			var category = await _categoryService.GetCategoryByIdAsync(categoryId);
			if (category is null)
			{
				return NotFound();
			}

			var categoryName = category.Name;
			int productPage = 1;

			return View(new ProductListViewModel
			{
				// TODO: Replace with category id.
				Products = productRepository.Products.Include(p => p.Category)
					.Where(p => categoryName == null || p.Category.Name == categoryName)
					.OrderBy(p => p.ProductId)
					.Skip((productPage - 1) * PageSize)
					.Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = productPage,
					ItemsPerPage = PageSize,
					TotalItems = categoryName == null
						? productRepository.Products.Count()
						: productRepository.Products.Include(p => p.Category).Where(e => e.Category.Name == categoryName).Count()
				},
				CurrentCategory = categoryName
			});
		}
	}
}
