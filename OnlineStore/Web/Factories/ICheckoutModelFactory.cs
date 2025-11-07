using GlideBuy.Models;
using GlideBuy.Web.Models.Checkout;

namespace GlideBuy.Web.Factories
{
	public interface ICheckoutModelFactory
	{
		Task<OnePageCheckoutModel> PrepareOnePageCheckoutModelAsync(IList<ShoppingCartItem> cart);
	}
}
