using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models;

namespace GlideBuy.Services.Orders
{
	public class OrderProcessingService : IOrderProcessingService
	{
		private readonly OrderSettings _orderSettings;
		private readonly IOrderTotalCalculationService _orderTotalCalculationService;

		public OrderProcessingService(
			OrderSettings orderSettings,
			IOrderTotalCalculationService orderTotalCalculationService)
		{
			_orderSettings = orderSettings;
			_orderTotalCalculationService = orderTotalCalculationService;
		}

		public async Task<bool> IsPaymentRequired(IList<ShoppingCartItem> cart, bool? useRewardPoints = null)
		{
			ArgumentNullException.ThrowIfNull(cart);

			var result = true;

			var total = (await _orderTotalCalculationService.GetShoppingCartTotalAsync(cart)).shoppingCartTotal;

			if (total is decimal.Zero)
			{
				result = false;
			}

			return result;
		}

		public async Task<bool> ValidateMinOrderSubtotalAmountAsync(IList<ShoppingCartItem> cart)
		{
			ArgumentNullException.ThrowIfNull(cart);

			if (!cart.Any() || _orderSettings.MinOrderSubtotalAmount <= decimal.Zero)
			{
				return true;
			}

			// TODO: Calculate subTotalWithoutDiscountBase

			return true;
		}
	}
}
