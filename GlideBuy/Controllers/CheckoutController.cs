using Microsoft.AspNetCore.Mvc;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Data.Repositories;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.Checkout;
using GlideBuy.Core.Domain.Common;
using GlideBuy.Services.Common;
using GlideBuy.Services.Customers;
using GlideBuy.Core;
using GlideBuy.Support.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace GlideBuy.Controllers
{
	public class CheckoutController : BaseController
	{
		private OrderRepository repository;
		private IShoppingCartService _shoppingCartService;
		private OrderSettings _orderSettings;
		private ICheckoutModelFactory _checkoutModelFactory;
		private IAddressService _addressService;
		private ICustomerService _customerService;
		private IWorkContext _workContext;
		private IOrderProcessingService _orderProcessingService;

		public CheckoutController(
			OrderRepository orderRepository,
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings,
			ICheckoutModelFactory checkoutModelFactory,
			IAddressService addressService,
			ICustomerService customerService,
			IWorkContext workContext,
			IOrderProcessingService orderProcessingService,
			IRazorViewEngine viewEngine) : base(viewEngine)
		{
			this.repository = orderRepository;
			_shoppingCartService = shoppingCartService;
			_orderSettings = orderSettings;
			_checkoutModelFactory = checkoutModelFactory;
			_addressService = addressService;
			_customerService = customerService;
			_workContext = workContext;
			_orderProcessingService = orderProcessingService;
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

			return RedirectToRoute("CheckoutDeliveryInformation");
		}

		#region Multi-Step Checkout

		// Step #1
		public async Task<IActionResult> DeliveryInformation()
		{
			// TODO: Check if checkout is disabled in the settings.

			var cart = await _shoppingCartService.GetShoppingCartAsync();

			if (!cart.Any())
			{
				return RedirectToRoute("ShoppingCart");
			}

			// TODO: Check if one step checkout is enabled.

			// TODO: Check if the user is signed in and if anonymous checkout is enabled.

			// TODO: Prepare the model.
			var model = new CheckoutDeliveryInformationModel();
			model.PostCode = string.Empty;

			return View(model);
		}

		[HttpPost, ActionName("DeliveryInformation")]
		[FormValueRequired("nextstep")]
		public async Task<IActionResult> NewDeliveryInformation(CheckoutDeliveryInformationModel model, IFormCollection form)
		{
			// TODO: Check if checkout is disabled.
			// TODO: Check for an empty cart.
			// TODO: Check if one page checkout is enabled.

			if (ModelState.IsValid)
			{
				// TODO: Store the postal code and associate it with the customer.

				return RedirectToRoute("CheckoutShippingMethod");
			}

			// TODO: If we got this far, then something is wrong. Re-prepare the model?
			return View(model);
		}

		public async Task<IActionResult> ShippingMethod()
		{
			// TODO: Check if checkout is disabled.
			// TODO: Check for an empty cart.
			// TODO: Check if one page checkout is enabled.
			// TODO: Check if anonymous checkout is enabled.
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			// TODO: Check if the shopping cart requires shipping (this usually true in electronics stores).

			// TODO: Retrieves the saved postal code of the customer. Or find another way.
			var model = await _checkoutModelFactory.PrepareShippingMethodModelAsync(cart, "H1 1AG");

			// TODO: If there's only one shipping method, bypass this step (check if the settings
			// allows this first).

			return View(model);
		}

		[HttpPost, ActionName("ShippingMethod")]
		[FormValueRequired("nextstep")]
		public async Task<IActionResult> SelectShippingMethod(IFormCollection form)
		{
			// TODO: Validation.

			//if (string.IsNullOrEmpty(shippingOption))
			//{
			//	return await ShippingMethod();
			//}

			// TODO: Check if "pickup from store" was selected.
			bool pickupFromStore = true;

			if (pickupFromStore)
			{
				return RedirectToRoute("CheckoutPaymentMethod");
			}

			return RedirectToRoute("CheckoutShippingAddress");
		}

		public async Task<IActionResult> ShippingAddress()
		{
			return View();
		}

		public async Task<IActionResult> PaymentMethod()
		{
			// TODO: Check if checkout is disabled.
			// TODO: Check for an empty cart.
			// TODO: Check if one page checkout is enabled.
			// TODO: Check if anonymous checkout is enabled.
			var cart = await _shoppingCartService.GetShoppingCartAsync();


			var paymentMethodModel = await _checkoutModelFactory.PreparePaymentMethodModelAsync(cart);

			// TODO: This section cannot be bypassed, since the information is entered in this step.

			return View(paymentMethodModel);
		}


		#endregion

		#region One Page Checkout (WIP / To be redesigned)

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

		[HttpPost]
		public async Task<IActionResult> OpcSaveBillingInformation(CheckoutBillingAddressModel model, IFormCollection form)
		{
			try
			{
				if (_orderSettings.CheckoutDisabled)
				{
					throw new Exception("Checkout is disabled");
				}

				var customer = await _workContext.GetCurrentCustomerAsync();
				var cart = await _shoppingCartService.GetShoppingCartAsync();

				_ = int.TryParse(form["billing_address_id"], out var billingAddressId);

				if (billingAddressId > 0) // existing address
				{
					// TODO: Get customer by id and update it.
				}
				else
				{
					var newAddressModel = model.BillingNewAddress;

					// Handle custom attributes

					if (!ModelState.IsValid)
					{
						var billingAddressModel = new CheckoutBillingAddressModel();
						await _checkoutModelFactory.PrepareBillingAddressModelAsync(billingAddressModel, cart);

						return Json(new
						{
							update_section = new
							{
								name = "billing",
								html = await RenderPartialViewToStringAsync("OpcBillingAddress", billingAddressModel)
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
					Address? address = null;

					if (address == null)
					{
						address = newAddressModel.ToEntity();
						address.CreateOnUtc = DateTime.UtcNow;

						await _addressService.InsertAddressAsync(address);
						await _customerService.InsertCustomerAddressAsync(customer, address);
					}

					//TODO: Update customer
					await _customerService.UpdateCustomerAsync(customer);
				}

				// TODO: Handle shipping in the future

				// Load the next step since shipping is not available.
				return await OpcLoadStepAfterShippingMethod(cart);
			}
			catch (Exception ex)
			{
				// TODO: Log the exception.

				return Json(new 
				{
					error = true,
					message = ex.Message
				});
			}
		}

		private async Task<JsonResult> OpcLoadStepAfterShippingMethod(IList<ShoppingCartItem> cart)
		{
			var customer = await _workContext.GetCurrentCustomerAsync();

			var isPaymentRequired = await _orderProcessingService.IsPaymentRequired(cart);

			if (isPaymentRequired)
			{
				return Json(new
				{
					goto_section = "payment_method"
				});
			}

			return Json(new
			{
				goto_section = "confirm_order"
			});
		}

		#endregion
	}
}
