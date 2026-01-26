using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models.ShoppingCart;

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

		Task<OrderSummaryModel> PrepareOrderSummaryModelAsync(bool isCartPage);

		Task<OrderTotalsModel> PrepareOrderTotalsModelAsync(
			IList<ShoppingCartItem> cart,
			bool isEditable);

		Task<MiniShoppingCartModel> PrepareMiniShoppingCartModelAsync();
	}
}
