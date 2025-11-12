using Microsoft.AspNetCore.Mvc;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Data.Repositories;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Factories;

namespace GlideBuy.Controllers
{
	public class CheckoutController : Controller
	{
		private OrderRepository repository;
		private IShoppingCartService _shoppingCartService;
		private OrderSettings _orderSettings;
		private ICheckoutModelFactory _checkoutModelFactory;

		public CheckoutController(
			OrderRepository orderRepository,
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings,
			ICheckoutModelFactory checkoutModelFactory)
		{
			this.repository = orderRepository;
			_shoppingCartService = shoppingCartService;
			_orderSettings = orderSettings;
			_checkoutModelFactory = checkoutModelFactory;
		}

		public async Task<IActionResult> Index()
		{
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			if (!cart.Any())
			{
				return RedirectToRoute("ShoppingCart");
			}

			// TODO: Check if the customer is guest and anonymous checkout is not enabled

			// Retrieve information about all the payment methods

			if (_orderSettings.OnePageCheckoutEnabled)
			{
				return RedirectToRoute("CheckoutOnePage");
			}

			return RedirectToRoute("CheckoutBillingAddress");
		}

		public async Task<IActionResult> OnePageCheckout()
		{
			if (_orderSettings.CheckoutDisabled)
			{
				return RedirectToRoute("ShoppingCart");
			}

			var cart = await _shoppingCartService.GetShoppingCartAsync();

			if (!cart.Any())
			{
				return RedirectToRoute("ShoppingCart");
			}

			if (!_orderSettings.OnePageCheckoutEnabled)
			{
				return RedirectToRoute("Checkout");
			}

			var model = await _checkoutModelFactory.PrepareOnePageCheckoutModelAsync(cart);

			return View(model);
		}

		public ViewResult ShippingAddress()
		{
			return View(new Order());
		}

		[HttpPost]
		public async Task<IActionResult> ShippingAddress(Order order)
		{
			var cart = await _shoppingCartService.GetShoppingCartAsync();

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
