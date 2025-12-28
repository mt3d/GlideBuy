using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageTrendingProductsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
