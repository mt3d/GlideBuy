using GlideBuy.Web.Models.ShoppingCart;
using GlideBuy.Models;

namespace GlideBuy.Web.Factories
{
	public interface IShoppingCartModelFactory
	{
		Task<ShoppingCartModel> PrepareShoppingCartModelAsync(
			ShoppingCartModel model,
			IList<ShoppingCartItem> cart,
			bool isEditable = true,
			bool validateCheckoutAttributes = false,
			bool prepareAndDisplayOrderReviewData = false);
	}
}
