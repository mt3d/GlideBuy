using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageSaleBannerViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
