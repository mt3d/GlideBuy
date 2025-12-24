using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageBestsellerViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
