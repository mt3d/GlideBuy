using GlideBuy.Services.Catalog;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageTrendingProductsViewComponent : ViewComponent
	{
		private readonly IProductService _productService;
		private readonly IProductModelFactory _productModelFactory;

		public HomePageTrendingProductsViewComponent(
			IProductService productService,
			IProductModelFactory productModelFactory)
		{
			_productService = productService;
			_productModelFactory = productModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var products = await _productService.GetHomepageTrendingProductsAsync(8);

			var models = (await _productModelFactory.PrepareProductOverviewModelsAsync(products)).ToList();

			return View(models);
		}
	}
}
