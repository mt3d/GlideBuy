using GlideBuy.Core.Domain.Orders;
using GlideBuy.Core.Domain.Shipping;
using GlideBuy.Models;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Models.Checkout;

namespace GlideBuy.Web.Factories
{
	public class CheckoutModelFactory : ICheckoutModelFactory
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly OrderSettings _orderSettings;
		private readonly ShippingSettings _shippingSettings;
		private readonly IAddressModelFactory _addressModelFactory;

		public CheckoutModelFactory(
			IShoppingCartService shoppingCartService,
			OrderSettings orderSettings,
			ShippingSettings shippingSettings,
			IAddressModelFactory addressModelFactory)
		{
			_shoppingCartService = shoppingCartService;
			_orderSettings = orderSettings;
			_shippingSettings = shippingSettings;
			_addressModelFactory = addressModelFactory;
		}

		public async Task<OnePageCheckoutModel> PrepareOnePageCheckoutModelAsync(IList<ShoppingCartItem> cart)
		{
			var model = new OnePageCheckoutModel()
			{
				ShippingRequired = await _shoppingCartService.ShoppingCartRequiresShippingAsync(cart),
				DisableBillingAddressCheckoutStep = _orderSettings.DisableBillingAddressCheckoutStep // TODO: Check if the customer has saved addresses.
			};

			await PrepareBillingAddressModelAsyn(model.BillingAddress, cart);

			return model;
		}

		public async Task PrepareBillingAddressModelAsyn(
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
	}
}
