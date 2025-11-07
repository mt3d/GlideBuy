using GlideBuy.Models;
using GlideBuy.Web.Models.Checkout;

namespace GlideBuy.Web.Factories
{
	public class CheckoutModelFactory : ICheckoutModelFactory
	{
		public Task<OnePageCheckoutModel> PrepareOnePageCheckoutModelAsync(IList<ShoppingCartItem> cart)
		{
			return Task.FromResult(new OnePageCheckoutModel());
		}
	}
}
