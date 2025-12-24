using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageListViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
