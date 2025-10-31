using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using GlideBuy.Models;

namespace GlideBuy.Components
{
	public class OrderSummaryViewComponent : ViewComponent
	{
		private IShoppingCartModelsFactory shoppingCartModelFactory;

		public OrderSummaryViewComponent(
			IShoppingCartModelsFactory shoppingCartModelFactory,
			Cart cartService)
		{
			this.shoppingCartModelFactory = shoppingCartModelFactory;

			CartService = cartService;
		}

		public Cart CartService { get; set; }

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

			// TODO: Move the cart to a dedicated service
			// var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
			IList<ShoppingCartItem> cartItems = CartService.Lines;

			// Create the shopping cart model
			ShoppingCartModel model = new();
			model = await shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, cartItems, isEditable: false);
			return View(model);
		}
	}
}
