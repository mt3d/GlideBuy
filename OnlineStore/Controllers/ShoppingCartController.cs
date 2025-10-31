using Microsoft.AspNetCore.Mvc;
using GlideBuy.Models;
using GlideBuy.Services.ProductCatalog;
using GlideBuy.Web.Models.ShoppingCart;
using GlideBuy.Web.Factories;

namespace GlideBuy.Controllers
{
	public class ShoppingCartController : Controller
	{
		private IProductService productService;
		private IShoppingCartModelsFactory _shoppingCartModelFactory;

		public ShoppingCartController(
			IProductService productService,
			IShoppingCartModelsFactory shoppingCartModelFactory,
			Cart cartService) // The cart stored in the session is added as a scoped service
		{
			this.productService = productService;
			_shoppingCartModelFactory = shoppingCartModelFactory;

			CartService = cartService;
		}

		public Cart CartService { get; set; }

		public async Task<IActionResult> Cart(string returnUrl)
		{
			//return View("Cart", new ShoppingCartModel { ReturnUrl = returnUrl ?? "/", Cart = CartService });
			
			// TODO: Read the shopping cart from a shopping cart service

			var model = new ShoppingCartModel();
			model = await _shoppingCartModelFactory.PrepareShoppingCartModelAsync(model, CartService.Lines);
			return View(model);
		}

		[HttpPost]
		public IActionResult AddProduct(long productId, string returnUrl)
		{
			Product? product = productService.GetProductById(productId);

			if (product != null)
			{
				CartService.AddItem(product, 1);
			}

			return RedirectToAction("Cart", new { returnUrl = returnUrl });
		}

		[HttpPost]
		public IActionResult RemoveProduct(long productId, string returnUrl)
		{
			CartService.RemoveLine(CartService.Lines.First(cl => cl.Product.ProductId == productId).Product);

			return RedirectToAction("Cart", new { returnUrl = returnUrl });
		}
	}
}
