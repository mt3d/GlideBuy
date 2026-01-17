using GlideBuy.Plugins.CreditCard.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Plugins.CreditCard.Components.PaymentCreditCard
{
	public class PaymentCreditCardViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = new PaymentInfoModel();

			return View("/Plugins/CreditCard/Components/PaymentCreditCard/Default.cshtml", model);
		}
	}
}