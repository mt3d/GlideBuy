using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageHeroSliderViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
