using GlideBuy.Core.Domain.Orders;
using GlideBuy.Core.Domain.Shipping;
using GlideBuy.Models;
using GlideBuy.Services.Orders;
using GlideBuy.Services.Payments;
using GlideBuy.Web.Models.Checkout;

namespace GlideBuy.Web.Factories
{
	public class CheckoutModelFactory : ICheckoutModelFactory
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly OrderSettings _orderSettings;
		private readonly ShippingSettings _shippingSettings;
		private readonly IAddressModelFactory _addressModelFactory;
		private readonly IPaymentPluginManager _paymentPluginManager;

		public CheckoutModelFactory(
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings,
			ShippingSettings shippingSettings,
			IAddressModelFactory addressModelFactory,
			IPaymentPluginManager paymentPluginManager)
		{
			_shoppingCartService = shoppingCartService;
			_orderSettings = orderSettings;
			_shippingSettings = shippingSettings;
			_addressModelFactory = addressModelFactory;
			_paymentPluginManager = paymentPluginManager;
		}

		// TODO: WIP. Redesign.
		public async Task<OnePageCheckoutModel> PrepareOnePageCheckoutModelAsync(IList<ShoppingCartItem> cart)
		{
			var model = new OnePageCheckoutModel()
			{
				ShippingRequired = await _shoppingCartService.ShoppingCartRequiresShippingAsync(cart),
				DisableBillingAddressCheckoutStep = _orderSettings.DisableBillingAddressCheckoutStep // TODO: Check if the customer has saved addresses.
			};

			await PrepareBillingAddressModelAsync(model.BillingAddress, cart);

			return model;
		}

		// TODO: WIP. Redesign.
		public async Task PrepareBillingAddressModelAsync(
			CheckoutBillingAddressModel model,
			IList<ShoppingCartItem> cart,
			int? selectedCountryId = null,
			bool prePopulateNewAddressWithCustomerFields = false)
		{
			ArgumentNullException.ThrowIfNull(model);

			model.ShipToSameAddressAllowed = _shippingSettings.ShipToSameAddress
				&& await _shoppingCartService.ShoppingCartRequiresShippingAsync(cart);

			// If "Billing Address" step is disabled, then customers cannot choose a billing address
			// and the same should apply to the shipping address.
			model.ShipToSameAddress = !_orderSettings.DisableBillingAddressCheckoutStep;

			// TODO: Handle VAT

			// TODO: Get all the addresses of the customer

			model.BillingNewAddress.CountryId = selectedCountryId;
			await _addressModelFactory.PrepareAddressModelAsync(
				model.BillingNewAddress,
				address: null,
				loadCountries: null,
				prePopulateWithCustomerFields: prePopulateNewAddressWithCustomerFields);
		}

		public async Task<CheckoutShippingMethodModel> PrepareShippingMethodModelAsync(IList<ShoppingCartItem> cart, string postalCode)
		{
			var model = new CheckoutShippingMethodModel();

			// TODO: Prepare the shipping model using the shipping service.

			return model;
		}

		public async Task<CheckoutPaymentMethodModel> PreparePaymentMethodModelAsync(IList<ShoppingCartItem> cart)
		{
			var model = new CheckoutPaymentMethodModel();

			// TODO: filter by country and customer.
			// TODO: filter by payment method. Only Standard or Redirection
			// TODO: Check if some methods should be hidden => HidePaymentMethodAsync().
			var paymentMethods = await _paymentPluginManager.LoadActivePluginsAsync();

			foreach (var pm in paymentMethods)
			{
				// TODO: Check if the shopping cart is 'recurring' and this method does not
				// support recurring payments. Return in this case.

				var pmModel = new CheckoutPaymentMethodModel.PaymentMethodModel
				{
					// TODO: Localized. Support localizing plugins friendly names.
					// TODO: Get the name from the plugin descriptor.
					Name = pm.PluginDescriptor.SystemName,

					// TODO: Should be removed?
					Description = await pm.GetPaymentMethodDescriptionAsync(),

					// TODO: Handle logo.

					PaymentViewComponent = pm.GetPublicViewComponent()
				};

				// TODO: Handle additional fees.

				model.PaymentMethods.Add(pmModel);
			}


			// TODO: Check if the customer has selected one of these methods before and
			// select it.

			if (model.PaymentMethods.FirstOrDefault(p => p.Selected) == null)
			{
				var paymentMethodToSelect = model.PaymentMethods.FirstOrDefault();
				if (paymentMethodToSelect != null)
				{
					paymentMethodToSelect.Selected = true;
				}
			}

			return model;
		}

		//public Task<> PreparePaymentInfoModelAsync(IPaymentMethod paymentMethod)
		//{

		//}
	}
}
