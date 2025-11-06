using Microsoft.AspNetCore.Mvc;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Data.Repositories;
using GlideBuy.Services.Orders;

namespace GlideBuy.Controllers
{
	public class CheckoutController : Controller
	{
		private OrderRepository repository;
		private IShoppingCartService _shoppingCartService;

		public CheckoutController(OrderRepository repositoryService, IShoppingCartService shoppingCartService)
		{
			this.repository = repositoryService;
			_shoppingCartService = shoppingCartService;
		}

		public ViewResult Index()
		{
			return View(new Order());
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(Order order)
		{
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			if (cart.Count() == 0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty!");
			}
			if (ModelState.IsValid)
			{
				order.Lines = cart.ToArray();
				repository.Save(order);
				cart.Clear();
				return RedirectToPage("/Completed", new { orderId = order.OrderId });
			} else
			{
				return View();
			}
		}
	}
}
