using OnlineStore.Models;

namespace GlideBuy.Services.Orders
{
	public interface IOrderProcessingService
	{
		Task<bool> ValidateMinOrderSubtotalAmountAsync(IList<ShoppingCartItem> cart);
	}
}
