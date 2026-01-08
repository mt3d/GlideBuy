using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class EstimateShippingViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
