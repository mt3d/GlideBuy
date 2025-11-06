using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using GlideBuy.Models;
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
		public async Task<IViewComponentResult> InvokeAsync(
			bool? prepareAndDisplayOrderReviewData,
			ShoppingCartModel overriddenModel)
		{
			if (overriddenModel is not null)
			{
				return View(overriddenModel);
			}

			// Get current store

			// TODO: Expand the parameters of GetShoppingCartAsync()
			IList<ShoppingCartItem> cartItems = await _shoppingCartService.GetShoppingCartAsync();

			// Create the shopping cart model
			ShoppingCartModel model = new();
			model = await shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, cartItems, isEditable: false);
			return View(model);
		}
	}
}
