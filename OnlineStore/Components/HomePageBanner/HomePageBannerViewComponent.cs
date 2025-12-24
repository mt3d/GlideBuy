using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageBannerViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
