using GlideBuy.Core.Domain.Orders;

namespace GlideBuy.Services.Orders
{
	public class OrderTotalCalculationService : IOrderTotalCalculationService
	{
		// TODO: Create a real world implementation.
		public async Task<(decimal? shoppingCartTotal, decimal discountAmount)> GetShoppingCartTotalAsync(IList<ShoppingCartItem> cart)
		{
			decimal total = 0;
			decimal discount = 0;

			foreach(var item in cart)
			{
				total += item.Product.Price * item.Quantity;
			}

			return (total, discount);
		}
	}
}
