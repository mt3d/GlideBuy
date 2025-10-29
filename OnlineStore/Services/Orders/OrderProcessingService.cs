using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models;

namespace GlideBuy.Services.Orders
{
	public class OrderProcessingService : IOrderProcessingService
	{
		private readonly OrderSettings orderSettings;

		public OrderProcessingService(OrderSettings orderSettings)
		{
			this.orderSettings = orderSettings;
		}

		public async Task<bool> ValidateMinOrderSubtotalAmountAsync(IList<ShoppingCartItem> cart)
		{
			ArgumentNullException.ThrowIfNull(cart);

			if (!cart.Any() || orderSettings.MinOrderSubtotalAmount <= decimal.Zero)
			{
				return true;
			}

			// TODO: Calculate subTotalWithoutDiscountBase

			return true;
		}
	}
}
