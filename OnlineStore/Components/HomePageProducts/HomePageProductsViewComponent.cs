using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageProductsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
