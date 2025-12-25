using GlideBuy.Core.Domain.Orders;

namespace GlideBuy.Services.Orders
{
	public interface IOrderProcessingService
	{
		Task<bool> ValidateMinOrderSubtotalAmountAsync(IList<ShoppingCartItem> cart);

		/// <summary>
		/// Payment might not be required if the order total is zero.
		/// </summary>
		/// <param name="cart"></param>
		/// <param name="useRewardPoints"></param>
		/// <returns></returns>
		Task<bool> IsPaymentRequired(IList<ShoppingCartItem> cart, bool? useRewardPoints = null);
	}
}
