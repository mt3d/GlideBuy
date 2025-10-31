using GlideBuy.Web.Models.ShoppingCart;
using GlideBuy.Models;

namespace GlideBuy.Web.Factories
{
	public interface IShoppingCartModelsFactory
	{
		Task<ShoppingCartModel> PrepareShoppingCartModelAsync(
			ShoppingCartModel model,
			IList<ShoppingCartItem> cart,
			bool isEditable = true,
			bool validateCheckoutAttributes = false,
			bool prepareAndDisplayOrderReviewData = false);

		Task<OrderTotalsModel> PrepareOrderTotalsModelAsync(
			IList<ShoppingCartItem> cart,
			bool isEditable);
	}
}
