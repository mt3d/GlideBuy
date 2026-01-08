using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageSpecialOffersViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
