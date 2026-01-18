using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Domain.Directory;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Core.Domain.Payment;
using GlideBuy.Models;
using GlideBuy.Services.Common;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Payments;
using System.Text.Json;

namespace GlideBuy.Services.Orders
{
	// One of the longest services in the whole system.
	public class OrderProcessingService : IOrderProcessingService
	{
		private readonly OrderSettings _orderSettings;
		private readonly IOrderTotalCalculationService _orderTotalCalculationService;
		private readonly IWorkContext _workContext;
		private readonly ICustomerService _customerService;
		private readonly IGenericAttributeService _genericAttributeService;
		private readonly PaymentSettings _paymentSettings;

		public OrderProcessingService(
			OrderSettings orderSettings,
			IOrderTotalCalculationService orderTotalCalculationService,
			IWorkContext workContext,
			ICustomerService customerService,
			IGenericAttributeService genericAttributeService,
			PaymentSettings paymentSettings)
		{
			_orderSettings = orderSettings;
			_orderTotalCalculationService = orderTotalCalculationService;
			_workContext = workContext;
			_customerService = customerService;
			_genericAttributeService = genericAttributeService;
			_paymentSettings = paymentSettings;
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

		public async Task<OrderPaymentContext?> GetOrderPaymentContextAsync()
		{
			var customer = await _workContext.GetCurrentCustomerAsync();
			var json = await _genericAttributeService.GetAttributeAsync<string>(customer, "OrderPaymentContext");

			return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<OrderPaymentContext>(json);
		}

		public async Task SetOrderPaymentContext(OrderPaymentContext orderPaymentContext, bool useNewOrderGuid = false)
		{
			var customer = await _workContext.GetCurrentCustomerAsync();

			if (orderPaymentContext is null)
			{
				// TODO: Move the string to a static class.
				await _genericAttributeService.SaveAttributeAsync<string>(customer, "OrderPaymentContext", null);

				return;
			}

			/**
			 * This if statement is entered when the two conditions are true.
			 * 
			 * The first is that _paymentSettings.RegenerateOrderGuidInterval is greater than zero, which means that the system is configured to allow reuse of an existing order GUID for a certain period of time. If this value were zero or negative, the whole reuse mechanism would be disabled and every payment attempt would implicitly use a new GUID.
			 * 
			 * The second condition is !useNewOrderGuid, which is an explicit override flag passed to the method. If the caller sets useNewOrderGuid to true, then the reuse logic is bypassed even if the interval setting would normally allow reuse.
			 */
			if (_paymentSettings.RegenerateOrderGuidInterval > 0 && !useNewOrderGuid)
			{
				var previousPaymentContext = await GetOrderPaymentContextAsync();

				/**
				 * This expressions uses pattern matching, specifically property patterns,
				 * together with the constant pattern not null. These features were introduced
				 * and expanded across C# 8.0 and C# 9.0.
				 * 
				 * 'is' here introduces a pattern. The pattern specify a shape.
				 * The pattern says: “match any object that has a property named
				 * OrderGuidGeneratedOnUtc whose value is not null.”
				 * 
				 * Implicitly, this also performs a null check on previousPaymentRequest itself.
				 * If previousPaymentRequest were null, the pattern could not be matched, and
				 * the condition would evaluate to false. So this single expression replaces
				 * two explicit checks: checking that the object is not null, and checking
				 * that a particular property on it is not null.
				 */
				if (previousPaymentContext is { OrderGuidGeneratedOnUtc: not null })
				{
					var interval = DateTime.UtcNow - previousPaymentContext.OrderGuidGeneratedOnUtc;

					if (interval.Value.TotalSeconds< _paymentSettings.RegenerateOrderGuidInterval)
					{
						orderPaymentContext.OrderGuid = previousPaymentContext.OrderGuid;
						orderPaymentContext.OrderGuidGeneratedOnUtc = previousPaymentContext.OrderGuidGeneratedOnUtc;
					}
				}
			}

			var json = JsonSerializer.Serialize(orderPaymentContext);
			await _genericAttributeService.SaveAttributeAsync(customer, "OrderPaymentContext", json);
		}

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
