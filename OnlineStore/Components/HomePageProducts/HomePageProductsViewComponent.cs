using GlideBuy.Models.Catalog;
using GlideBuy.Services.Catalog;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageProductsViewComponent : ViewComponent
	{
		private readonly IProductService _productService;
		private readonly IProductModelFactory _productModelFactory;

		public HomePageProductsViewComponent(
			IProductService productService,
			IProductModelFactory productModelFactory)
		{
			_productService = productService;
			_productModelFactory = productModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// TODO: Handle ACL permissions.
			// TODO: Handle product availability using ProductService.
			// TODO: Handle grouped products in the future.
			var products = await _productService.GetAllProductsDisplayedOnHomepageAsync();

			if (!products.Any())
			{
				return Content("");
			}

			var model = (await _productModelFactory.PrepareProductOverviewModelsAsync(products)).ToList();

			return View(model);
		}
	}
}
