using Microsoft.AspNetCore.Mvc;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Data.Repositories;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.Checkout;
using GlideBuy.Core.Domain.Common;
using GlideBuy.Services.Common;
using GlideBuy.Services.Customers;

namespace GlideBuy.Controllers
{
	public class CheckoutController : Controller
	{
		private OrderRepository repository;
		private IShoppingCartService _shoppingCartService;
		private OrderSettings _orderSettings;
		private ICheckoutModelFactory _checkoutModelFactory;
		private IAddressService _addressService;
		private ICustomerService _customerService;

		public CheckoutController(
			OrderRepository orderRepository,
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings,
			ICheckoutModelFactory checkoutModelFactory,
			IAddressService addressService,
			ICustomerService customerService)
		{
			this.repository = orderRepository;
			_shoppingCartService = shoppingCartService;
			_orderSettings = orderSettings;
			_checkoutModelFactory = checkoutModelFactory;
			_addressService = addressService;
			_customerService = customerService;
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

		[HttpPost]
		public async Task<IActionResult> OpcSaveBillingInformation(CheckoutBillingAddressModel model, IFormCollection form)
		{
			try
			{
				if (_orderSettings.CheckoutDisabled)
				{
					throw new Exception("Checkout is disabled");
				}

				// var customer = await _workContext.GetCurrentCustomerAsync();

				_ = int.TryParse(form["billing_address_id"], out var billingAddressId);

				if (billingAddressId > 0) // existing address
				{

				}
				else
				{
					var newAddressModel = model.BillingNewAddress;

					// Handle custom attributes

					if (!ModelState.IsValid)
					{
						var billingAddressModel = new CheckoutBillingAddressModel();

						return Json(new
						{
							update_section = new
							{
								name = "billing"
							},
							wrong_billing_address = true,
							error = true,
							message = string.Join(", ",
								ModelState.Values
									.Where(p => p.Errors.Any())
									.SelectMany(p => p.Errors) // Merge all into IEnumerable<ModelError>
									.Select(p => p.ErrorMessage) // Select only ErrorMessage from the sequence elements
							)
						});
					}

					// The model is valid, create an address entity.

					// TODO: Search for the address first
					Address address = null;

					if (address == null)
					{
						address = newAddressModel.ToEntity();
						address.CreateOnUtc = DateTime.UtcNow;

						await _addressService.InsertAddressAsync(address);
						// await _customerService.InsertCustomerAddressAsync(customer, address);
					}

					//TODO: Update customer
				}
			}
			catch (Exception ex)
			{
				// TODO: Log the exception.

				return Json(new { error = 1, message = ex.Message });
			}

			throw new NotImplementedException();
		}
	}
}
