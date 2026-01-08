using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class SelectedCheckoutAttributesViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
