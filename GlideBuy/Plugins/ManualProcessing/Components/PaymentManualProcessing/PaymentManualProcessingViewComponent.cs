using GlideBuy.Plugins.CreditCard.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Plugins.CreditCard.Components.PaymentManualProcessing
{
	public class PaymentManualProcessingViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = new PaymentInfoModel();

			return View("/Plugins/ManualProcessing/Components/PaymentManualProcessing/Default.cshtml", model);
		}
	}
}