using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageProductOffersViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
