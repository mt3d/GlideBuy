using OnlineStore.Models;

namespace GlideBuy.Services.Orders
{
	public class OrderProcessingService : IOrderProcessingService
	{
		public async Task<bool> ValidateMinOrderSubtotalAmountAsync(IList<ShoppingCartItem> cart)
		{
			return true;
		}
	}
}
