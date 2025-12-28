using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageBrandsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
