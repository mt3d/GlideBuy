using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services.ProductCatalog;
using GlideBuy.Web.Models.ShoppingCart;

namespace OnlineStore.Controllers
{
	public class ShoppingCartController : Controller
	{
		private IProductService productService;

		public ShoppingCartController(
			IProductService productService,
			Cart cartService) // The cart stored in the session is added as a scoped service
		{
			this.productService = productService;
			CartService = cartService;
		}

		public Cart CartService { get; set; }

		public IActionResult Cart(string returnUrl)
		{
			return View("Cart", new ShoppingCartModel { ReturnUrl = returnUrl ?? "/", Cart = CartService });
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
