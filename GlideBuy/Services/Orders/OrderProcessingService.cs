using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models;
using GlideBuy.Services.Payments;

namespace GlideBuy.Services.Orders
{
	// One of the longest services in the whole system.
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

		#region Utilities

		#endregion

		#region Methods

		/**
		 * This is one of the most critical pieces of checkout infrastructure. It is
		 * intentionally dense because it sits at the intersection of payment processing,
		 * order persistence, inventory mutation, and system consistency guarantees.
		 * 
		 * The method begins with strict defensive validation. The ProcessPaymentRequest
		 * is mandatory, and the presence of a non-empty OrderGuid is enforced immediately.
		 * This GUID is generated earlier in the checkout flow and acts as an idempotency
		 * anchor. By refusing to proceed without it, the system ensures that order
		 * creation can be correlated, retried safely, and distinguished from duplicate
		 * submissions. This is one of the first signals that the method is designed with
		 * concurrency and replay scenarios in mind.
		 */
		public async Task PlaceOrderAsync(OrderPaymentContext? orderPaymentContext)
		{
			ArgumentNullException.ThrowIfNull(orderPaymentContext);

			if (orderPaymentContext.OrderGuid == Guid.Empty)
			{
				throw new Exception("Order GUID not generated");
			}
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

		#endregion
	}
}
