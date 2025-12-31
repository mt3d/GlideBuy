using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;
using GlideBuy.Services.Orders;

namespace GlideBuy.Components
{
	public class OrderSummaryViewComponent : ViewComponent
	{
		private IShoppingCartModelsFactory shoppingCartModelFactory;
		private IShoppingCartService _shoppingCartService;

		public OrderSummaryViewComponent(
			IShoppingCartModelsFactory shoppingCartModelFactory,
			IShoppingCartService shoppingCartService)
		{
			this.shoppingCartModelFactory = shoppingCartModelFactory;
			_shoppingCartService = shoppingCartService;
		}

		/// <summary>
		/// The caller can override the default shopping cart model. Sometimes, you
		/// would want the model to be editable, and sometimes not.
		/// </summary>
		/// <param name="prepareAndDisplayOrderReviewData"></param>
		/// <param name="overriddenModel"></param>
		/// <returns></returns>
		public async Task<IViewComponentResult> InvokeAsync(bool isCartPage)
		{
			var model = await shoppingCartModelFactory.PrepareOrderSummaryModelAsync(isCartPage: isCartPage);
			return View(model);
		}
	}
}
