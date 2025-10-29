using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using GlideBuy.Models;
using GlideBuy.Services.ProductCatalog;

namespace GlideBuy.Components
{
	public class OrderSummaryViewComponent : ViewComponent
	{
		private IShoppingCartModelFactory shoppingCartModelFactory;

		public OrderSummaryViewComponent(
			IShoppingCartModelFactory shoppingCartModelFactory,
			Cart cartService)
		{
			this.shoppingCartModelFactory = shoppingCartModelFactory;

			CartService = cartService;
		}

		public Cart CartService { get; set; }

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// Get current store

			// Get shopping cart
			// TODO: Move the cart to a dedicated service
			//         var cart = await _shoppingCartService.GetShoppingCartAsync(await _workContext.GetCurrentCustomerAsync(), ShoppingCartType.ShoppingCart, store.Id);
			IList<ShoppingCartItem> cartItems = CartService.Lines;

			// Create the shopping cart model
			ShoppingCartModel model = new();
			model = await shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, cartItems, isEditable: false);
			return View(model);
		}
	}
}
