using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Orders
{
	public class ShoppingCartItem
	{
		public int ShoppingCartItemId { get; set; }

		// TODO: Store the product or just the ID?
		public Product Product { get; set; } = new();

		public int Quantity { get; set; }
	}
}
