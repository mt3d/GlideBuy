using Microsoft.AspNetCore.Mvc;
using GlideBuy.Models;
using GlideBuy.Services.Orders;

namespace GlideBuy.Components
{
	public class CartSummaryViewComponent : ViewComponent
	{
		private IShoppingCartService _shoppingCartService;

		public CartSummaryViewComponent(IShoppingCartService shoppingCartService)
		{
			_shoppingCartService = shoppingCartService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			return View(cart);
		}
	}
}
