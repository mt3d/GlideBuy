using GlideBuy.Web.Models.ShoppingCart;
using OnlineStore.Models;

namespace GlideBuy.Web.Factories
{
	public class ShoppingCartModelFactory : IShoppingCartModelFactory
	{
		public async Task<ShoppingCartModel> PrepareShoppingCartModelAsync(
			ShoppingCartModel model,
			IList<ShoppingCartItem> cart,
			bool isEditable = true,
			bool validateCheckoutAttributes = false,
			bool prepareAndDisplayOrderReviewData = false)
		{
			return model;
		}
	}
}
