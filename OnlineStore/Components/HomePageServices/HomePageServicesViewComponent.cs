using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageServicesViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
