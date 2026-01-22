using GlideBuy.Services.Catalog;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageNewArrivalsViewComponent : ViewComponent
	{
		private readonly IProductService _productService;
		private readonly IProductModelFactory _productModelFactory;

		public HomePageNewArrivalsViewComponent(
			IProductService productService,
			IProductModelFactory productModelFactory)
		{
			_productService = productService;
			_productModelFactory = productModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var products = await _productService.GetNewlyArrivedProducts(8);

			var models = (await _productModelFactory.PrepareProductOverviewModelsAsync(products));

			return View(models);
		}
	}
}
