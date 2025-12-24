using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageCarouselViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
