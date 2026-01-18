using GlideBuy.Core.Domain.Orders;
using GlideBuy.Services.Payments;

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

		Task<PlaceOrderResult> PlaceOrderAsync(OrderPaymentContext? orderPaymentContext);

		Task SetOrderPaymentContext(OrderPaymentContext? orderPaymentContext, bool useNewOrderGuid = false);

		Task<OrderPaymentContext?> GetOrderPaymentContextAsync();
	}
}
