using GlideBuy.Models;

namespace GlideBuy.Services.Orders
{
	public interface IShoppingCartService
	{
		public Task<List<ShoppingCartItem>> GetShoppingCartAsync();

		public void AddToCartAsync(Product product, int quantity);

		public void DeleteShoppingCartItemAsync(Product product);

		public void ClearShoppingCartAsync();
	}
}
