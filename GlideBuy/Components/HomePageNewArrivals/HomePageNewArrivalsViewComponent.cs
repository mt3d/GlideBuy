using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageNewArrivalsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
