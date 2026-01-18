using GlideBuy.Core;
using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Services.Common;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Orders;
using GlideBuy.Services.Payments;
using GlideBuy.Support.Controllers;
using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.Checkout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;

namespace GlideBuy.Controllers
{
	public class CheckoutController : BaseController
	{
		private IShoppingCartService _shoppingCartService;
		private OrderSettings _orderSettings;
		private ICheckoutModelFactory _checkoutModelFactory;
		private IAddressService _addressService;
		private ICustomerService _customerService;
		private IWorkContext _workContext;
		private IOrderProcessingService _orderProcessingService;

		public CheckoutController(
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings,
			ICheckoutModelFactory checkoutModelFactory,
			IAddressService addressService,
			ICustomerService customerService,
			IWorkContext workContext,
			IOrderProcessingService orderProcessingService,
			IRazorViewEngine viewEngine) : base(viewEngine)
		{
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

		public async Task<IActionResult> Completed(int? orderId)
		{
			// TODO: Check if anonymous checkout is allowed.

			// TODO: Get the order by ID.
			// TODO: If the order is null, redirect to the homepage.

			// TODO: Create the model based on the order retrieved.
			var model = new CheckoutCompleteModel();

			return View(model);
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

			// TODO: Check if payment workflow is required using OrderProcessingService.
			// TODO: Get the customer country for filtering.

			var paymentMethodModel = await _checkoutModelFactory.PreparePaymentMethodModelAsync(cart);

			// TODO: This section cannot be bypassed, since we need the final confirmation
			// of the user anyway. Howeve, it could be skipped in the future, if there are
			// further steps.

			// TODO: Handle payment method information.

			return View(paymentMethodModel);
		}

		// Compared to Nop, this method is a merge of SelectPaymentMethod, EnterPaymentInfo, and ConfirmOrder.
		// TODO: Validate captcha.
		// TODO: Should be PaymentMethod, because in case of errors, we will get redirected to PaymentMethod.
		[HttpPost, ActionName("PaymentMethod")]
		public async Task<IActionResult> ConfirmPaymentMethodAndOrder(IFormCollection form)
		{
			// TODO: Validate
			// Check if checkout is disabled.
			// Check if cart is empty.
			// Check if anonymous checkout is enabled.

			var customer = await _workContext.GetCurrentCustomerAsync();
			var cart = await _shoppingCartService.GetShoppingCartAsync();
			var model = await _checkoutModelFactory.PreparePaymentMethodModelAsync(cart);

			// TODO: payment method name should be a parameter, and it should not be null.
			// First, check that the payment plugin is active. If not, reload.
			// Save the payment method as an attribute (SelectedPaymentMethodAttribute)
			// for next steps. In our case, there are none.
			// Load SelectedPaymentMethodAttribute.

			var paymentMethodSystemName = form["paymentmethod"];
			if (string.IsNullOrEmpty(paymentMethodSystemName))
			{
				return await PaymentMethod();
			}

			IPaymentMethod paymentMethod = null; // TODO: Load from PluginManager.
												 // IForm should be an argument.
												 // TODO: Validate the payment form using paymentMethod.ValidatePaymentFormAsync()
			var warnings = await paymentMethod.ValidatePaymentFormAsync(form);
			foreach (var warning in warnings)
			{
				ModelState.AddModelError("", warning);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await _orderProcessingService.SetOrderPaymentContext(await paymentMethod.GetPaymentInfoAsync(form));

			// TODO: Check if captcha is valid. If not realod.

			try
			{
				// TODO: Check for minimum placement interval.
				/**
				 * The core logic begins inside the try block, and this is where payment processing becomes relevant. The first check inside this block enforces the minimum order placement interval. This is an anti-double-submit mechanism that protects against rapid repeated clicks, browser retries, or malicious automation. If this interval is violated, the method throws an exception, which is then handled uniformly by the catch block and surfaced to the user as a warning.
				 */

				/**
				 * The next step is retrieving the ProcessPaymentRequest via _orderProcessingService.GetProcessPaymentRequestAsync(). This object is central to understanding NopCommerce’s payment architecture. It acts as a state container that survives across checkout steps, especially in scenarios where the payment method requires a redirect or additional information. If this request is null, the system checks whether a payment workflow is required at all. This covers cases such as zero-total orders, cash on delivery, or payment methods that do not require an intermediate payment info step. If a workflow is required, the user is redirected back to CheckoutPaymentInfo, because confirming the order without required payment data would be invalid. Otherwise, a new ProcessPaymentRequest is created.
				 */
				// TODO: Is there any need to use SetOrderPaymentContext() above?
				var orderPaymentContext = await _orderProcessingService.GetOrderPaymentContextAsync();
				orderPaymentContext.CustomerId = customer.Id;
				orderPaymentContext.PaymentMethodSystemName = paymentMethodSystemName;

				/**
				 * The populated request is then persisted using SetProcessPaymentRequestAsync, ensuring that downstream services and payment plugins can access it consistently.
				 */
				await _orderProcessingService.SetOrderPaymentContext(orderPaymentContext);

				/**
				 * This is the most critical call in the entire checkout process. Internally, this
				 * method performs a large number of operations in a controlled transaction-like sequence:
				 * validating the cart again, calculating totals, creating the order record,
				 * creating order items, adjusting inventory, applying discounts, and invoking
				 * the payment method’s ProcessPayment logic. The result of this operation is a
				 * PlaceOrderResult, which explicitly indicates success or failure rather than
				 * throwing exceptions for expected business errors.
				 */
				var placeOrderResult = await _orderProcessingService.PlaceOrderAsync(orderPaymentContext);

				if (placeOrderResult.Success)
				{
					// TODO: PostProcessPaymentAsync
					/**
					 * If the order placement succeeds, the stored ProcessPaymentRequest is immediately cleared. This is essential to prevent stale payment data from being reused if the customer later places another order.
					 */
					await _orderProcessingService.SetOrderPaymentContext(null);

					/**
					 * After that, the method initiates post-payment processing by creating a PostProcessPaymentRequest containing the placed order and passing it to _paymentService.PostProcessPaymentAsync. This is where payment methods that require redirection, external authorization, or form POSTs take control. Examples include redirecting the user to PayPal, Stripe-hosted pages, or bank gateways.
					*/
					// TODO: Post process payment.

					/**
					 * The subsequent check of _webHelper.IsRequestBeingRedirected and _webHelper.IsPostBeingDone is subtle but extremely important. It allows payment plugins to fully control the HTTP response. If the payment plugin already issued a redirect or POST response, the controller must stop execution immediately and return an empty result, otherwise ASP.NET MVC would attempt to write another response and cause runtime errors. If no redirection occurred, the flow continues normally and the customer is redirected to the checkout completion page.
					 */
					// TODO: Check redirection.

					return RedirectToRoute("CheckoutCompleted"); // TODO: Pass the order Id.
				}
				else
				{
					// TODO: Use ModelState.AddModelError instead? Inconsistency.
					foreach (var error in placeOrderResult.Errors)
					{
						model.Warnings.Add(error);
					}
				}
			}
			catch (Exception ex)
			{
				// TODO: Log the warning.

				model.Warnings.Add(ex.Message);
			}

			return View(model);
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
