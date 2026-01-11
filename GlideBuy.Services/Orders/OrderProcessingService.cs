using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Domain.Directory;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Payments;

namespace GlideBuy.Services.Orders
{
	// One of the longest services in the whole system.
	public class OrderProcessingService : IOrderProcessingService
	{
		private readonly OrderSettings _orderSettings;
		private readonly IOrderTotalCalculationService _orderTotalCalculationService;
		private readonly IWorkContext _workContext;
		private readonly ICustomerService _customerService;

		public OrderProcessingService(
			OrderSettings orderSettings,
			IOrderTotalCalculationService orderTotalCalculationService,
			IWorkContext workContext,
			ICustomerService customerService)
		{
			_orderSettings = orderSettings;
			_orderTotalCalculationService = orderTotalCalculationService;
			_workContext = workContext;
			_customerService = customerService;
		}

		#region Utilities

		protected async Task PrepareAndValidateCustomerAsync(PlaceOrderContainer details, OrderPaymentContext orderPaymentContext, Currency currency)
		{
			// OrderPaymentContext.CustomerId is set in the final checkout step.
			details.Customer = await _customerService.GetCustomerByIdAsync(orderPaymentContext.CustomerId);
		}

		protected async Task<PlaceOrderContainer> PreparePlaceOrderDetailsAsync(OrderPaymentContext orderPaymentContext)
		{
			var details = new PlaceOrderContainer();

			var currentCurrency = await _workContext.GetWorkingCurrencyAsync();
			await PrepareAndValidateCustomerAsync(details, orderPaymentContext, currentCurrency);

			// TODO: Add support for recurring payments and validate them.

			return details;
		}

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

			/**
			 * Next, PreparePlaceOrderDetailsAsync is invoked. This method constructs
			 * a PlaceOrderContainer, which is effectively a snapshot of all relevant
			 * checkout state at the exact moment of order placement. It aggregates the
			 * customer, store, cart items, totals, discounts, taxes, shipping data,
			 * checkout attributes, and flags such as whether the cart is recurring.
			 * This container is crucial because it decouples downstream logic from volatile
			 * sources such as the shopping cart service. Once this container exists, all
			 * subsequent operations rely on it rather than re-querying mutable state.
			 */

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

		#region Nested classes

		/// <summary>
		/// A snapshot of all relevant checkout state at the exact moment of order placement.
		/// </summary>
		protected class PlaceOrderContainer
		{
			public Customer Customer { get; set; }
		}

		#endregion
	}
}
