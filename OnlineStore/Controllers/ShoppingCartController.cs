using Microsoft.AspNetCore.Mvc;
using GlideBuy.Models;
using GlideBuy.Services.ProductCatalog;
using GlideBuy.Web.Models.ShoppingCart;
using GlideBuy.Web.Factories;
using GlideBuy.Support.Controllers;
using GlideBuy.Services.Orders;
using GlideBuy.Core.Domain.Orders;

namespace GlideBuy.Controllers
{
	public class ShoppingCartController : Controller
	{
		private IProductService productService;
		private IShoppingCartModelsFactory _shoppingCartModelFactory;
		private IShoppingCartService _shoppingCartService;
		private OrderSettings _orderSettings;

		public ShoppingCartController(
			IProductService productService,
			IShoppingCartModelsFactory shoppingCartModelFactory,
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings)
		{
			this.productService = productService;
			_shoppingCartModelFactory = shoppingCartModelFactory;
			_shoppingCartService = shoppingCartService;
			_orderSettings = orderSettings;
		}

		public async Task<IActionResult> Cart(string returnUrl)
		{			
			var model = new ShoppingCartModel();
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, cart);
			return View(model);
		}

		[HttpPost, ActionName("Cart")]
		[FormValueRequired("checkout")]
		public async Task<IActionResult> StartCheckout(IFormCollection form)
		{
			// TODO: Handle checkout attributes

			// TODO: Only enable if registration is disabled
			var anonymousPermissed = _orderSettings.AnonymousCheckoutDisabled;

			// TODO: Replace true with checking if the user is registered (not guest)
			if (anonymousPermissed || true)
			{
				return RedirectToRoute("Checkout");
			}

			// TODO: Handle downloadable produts

			if (!_orderSettings.AnonymousCheckoutDisabled)
			{
				return Challenge();
			}

			return RedirectToRoute("CheckoutAsGuest", new { returnUrl = Url.RouteUrl("ShoppingCart") });
		}

		[HttpPost]
		public IActionResult AddProduct(long productId, string returnUrl)
		{
			Product? product = productService.GetProductById(productId);

			if (product != null)
			{
				_shoppingCartService.AddToCartAsync(product, 1);
			}

			return RedirectToRoute("ShoppingCart", new { returnUrl = returnUrl });
		}

		[HttpPost]
		public async Task<IActionResult> RemoveProduct(long productId, string returnUrl)
		{
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			_shoppingCartService.DeleteShoppingCartItemAsync(cart.First(cl => cl.Product.ProductId == productId).Product);

			return RedirectToRoute("ShoppingCart", new { returnUrl = returnUrl });
		}
	}
}
