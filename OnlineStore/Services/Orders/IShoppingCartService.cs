using GlideBuy.Models;

namespace GlideBuy.Services.Orders
{
	public interface IShoppingCartService
	{
	    Task<List<ShoppingCartItem>> GetShoppingCartAsync();

		// TODO: Make async
		void AddToCartAsync(Product product, int quantity);

		// TODO: Make async
		void DeleteShoppingCartItemAsync(Product product);

		// TODO: Make async
		void ClearShoppingCartAsync();

		Task<bool> ShoppingCartRequiresShippingAsync(IList<ShoppingCartItem> cart);
	}
}
