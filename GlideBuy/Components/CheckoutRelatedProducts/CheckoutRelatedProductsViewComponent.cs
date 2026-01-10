using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components.CheckoutRelatedProducts
{
	public class CheckoutRelatedProductsViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
