using GlideBuy.Core.Domain.Orders;

namespace GlideBuy.Services.Orders
{
	public interface IOrderTotalCalculationService
	{
		Task<(decimal? shoppingCartTotal, decimal discountAmount)> GetShoppingCartTotalAsync(IList<ShoppingCartItem> cart);
	}
}
