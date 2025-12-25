using GlideBuy.Models.Catalog;
using GlideBuy.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageProductsViewComponent : ViewComponent
	{
		private readonly IProductService _productService;

		public HomePageProductsViewComponent(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var products = _productService.GetAllProductsDisplayedOnHomepageAsync();

			var model = new List<ProductOverviewModel>();

			return View(model);
		}
	}
}
