using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Plugins.CreditCard.Components.PaymentCreditCard
{
	public class PaymentCreditCardViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View("/Plugins/CreditCard/Components/PaymentCreditCard/Default.cshtml");
		}
	}
}