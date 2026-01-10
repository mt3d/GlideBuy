using GlideBuy.Core.Domain.Orders;
using GlideBuy.Web.Models.Checkout;

namespace GlideBuy.Web.Factories
{
	public interface ICheckoutModelFactory
	{
		Task<OnePageCheckoutModel> PrepareOnePageCheckoutModelAsync(IList<ShoppingCartItem> cart);

		Task PrepareBillingAddressModelAsync(
		   CheckoutBillingAddressModel model,
		   IList<ShoppingCartItem> cart,
		   int? selectedCountryId = null,
		   bool prePopulateNewAddressWithCustomerFields = false);

		Task<CheckoutShippingMethodModel> PrepareShippingMethodModelAsync(IList<ShoppingCartItem> cart, string postalCode);

		Task<CheckoutPaymentMethodModel> PreparePaymentMethodModelAsync(IList<ShoppingCartItem> cart);
	}
}
