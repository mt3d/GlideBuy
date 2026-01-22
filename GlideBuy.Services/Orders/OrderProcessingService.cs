using GlideBuy.Core;
using GlideBuy.Core.Caching;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Domain.Directory;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Core.Domain.Payment;
using GlideBuy.Models;
using GlideBuy.Services.Common;
using GlideBuy.Services.Customers;
using GlideBuy.Services.Payments;
using System.Diagnostics;
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
		private readonly IStaticCacheManager _staticCacheManger;
		private readonly IPaymentPluginManager _paymentPluginManager;
		private readonly IPaymentService _paymentService;

		public OrderProcessingService(
			OrderSettings orderSettings,
			IOrderTotalCalculationService orderTotalCalculationService,
			IWorkContext workContext,
			ICustomerService customerService,
			IGenericAttributeService genericAttributeService,
			PaymentSettings paymentSettings,
			IStaticCacheManager staticCacheManager,
			IPaymentPluginManager paymentPluginManager)
		{
			_orderSettings = orderSettings;
			_orderTotalCalculationService = orderTotalCalculationService;
			_workContext = workContext;
			_customerService = customerService;
			_genericAttributeService = genericAttributeService;
			_paymentSettings = paymentSettings;
			_staticCacheManger = staticCacheManager;
			_paymentPluginManager = paymentPluginManager;
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

		protected async Task<ProcessPaymentResult> GetProcessPaymentResult(OrderPaymentContext orderPaymentContext, PlaceOrderContainer container)
		{
			ProcessPaymentResult processPaymentResult;

			// TODO: Check if payment is required based on the content of the cart.

			// All the work is being done in the PaymentService. This is just for checking that
			// the payment method plugin exists and is active.
			var customer = await _customerService.GetCustomerByIdAsync(orderPaymentContext.CustomerId);
			var paymentMethod = _paymentPluginManager.LoadPluginBySystemNameAsync(orderPaymentContext.PaymentMethodSystemName) ?? throw new Exception("Payment method is not active");

			// TODO: Check that the payment plugin is active.

			// TODO: Check if the shopping cart is recurring.

			processPaymentResult = await _paymentService.ProcessPaymentAsync(orderPaymentContext);

			return processPaymentResult;
		}

		#endregion

		#region Methods

		public async Task<OrderPaymentContext?> GetOrderPaymentContextAsync()
		{
			var customer = await _workContext.GetCurrentCustomerAsync();
			var json = await _genericAttributeService.GetAttributeAsync<string>(customer, "OrderPaymentContext");

			return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<OrderPaymentContext>(json);
		}

		public async Task SetOrderPaymentContext(OrderPaymentContext? orderPaymentContext, bool useNewOrderGuid = false)
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
		 * This method is about guaranteeing that exactly one order is created for a given
		 * checkout attempt, even under retries, double clicks, network latency, or parallel requests.
		 */
		public async Task<PlaceOrderResult> PlaceOrderAsync(OrderPaymentContext? orderPaymentContext)
		{
			ArgumentNullException.ThrowIfNull(orderPaymentContext);

			/**
			 * The method begins with strict defensive validation. The ProcessPaymentRequest
			 * is mandatory, and the presence of a non-empty OrderGuid is enforced immediately.
			 * This GUID is generated earlier in the checkout flow and acts as an idempotency
			 * anchor. By refusing to proceed without it, the system ensures that order
			 * creation can be correlated, retried safely, and distinguished from duplicate
			 * submissions. This is one of the first signals that the method is designed with
			 * concurrency and replay scenarios in mind.
			 */
			if (orderPaymentContext.OrderGuid == Guid.Empty)
			{
				throw new Exception("Order GUID not generated");
			}

			/**
			 * This method constructs
			 * a PlaceOrderContainer, which is effectively a snapshot of all relevant
			 * checkout state at the exact moment of order placement. It aggregates the
			 * customer, store, cart items, totals, discounts, taxes, shipping data,
			 * checkout attributes, and flags such as whether the cart is recurring.
			 * This container is crucial because it decouples downstream logic from volatile
			 * sources such as the shopping cart service. Once this container exists, all
			 * subsequent operations rely on it rather than re-querying mutable state.
			 */
			var details = await PreparePlaceOrderDetailsAsync(orderPaymentContext);

			async Task<PlaceOrderResult> placeOrder(PlaceOrderContainer placeOrderContainer)
			{
				var result = new PlaceOrderResult();

				try
				{
					var processPaymentResult = await GetProcessPaymentResult(orderPaymentContext, placeOrderContainer) ?? throw new Exception("The result of process payment is not available.");

					if (processPaymentResult.Success)
					{
						// TODO: Save the order and assign it to the result.
					}
				}
				catch (Exception ex)
				{

				}

				if (result.Success)
				{
					return result;
				}

				// TODO: Add all errors to result and log them.

				return result;
			}

			if (!_orderSettings.PlaceOrderWithLock)
			{
				return await placeOrder(details);
			}

			PlaceOrderResult? result;
			var resource = details.Customer.Id.ToString();

			/**
			 * The mutex exists to prevent concurrent order placement for the same customer and shopping cart. In web applications, it is surprisingly easy for the same user to trigger multiple PlaceOrderAsync calls nearly simultaneously, for example by double clicking the confirm button, refreshing the page during a slow response, or when a reverse proxy retries a request. Without synchronization, two threads could pass all validation, process payment, and persist two separate orders for the same cart.
			 * 
			 * The mutex is named using the customer identifier, which means that it serializes order placement only per customer, not globally, and therefore does not degrade overall system throughput.
			 * 
			 * The mutex is intentionally named and system wide, so it works across threads and potentially across processes on the same machine, something that a simple in-memory lock would not guarantee.
			 * 
			 * SemaphoreSlim cannot be relied upon on all UNIX based environments in this context, and a regular C# lock would not protect against cross-thread or cross-request concurrency in the same way. A named mutex, although heavy, provides a simple and reliable cross-thread synchronization primitive that works at the operating system level.
			 * 
			 * mutexes cannot be used in with the await operation, which explains the otherwise awkward synchronous calls using .Result and .Wait(). Await would yield the thread while the mutex is held, potentially leading to deadlocks or starvation, so the code deliberately forces synchronous execution inside that critical section.
			 * 
			 * When you use await inside a method, you are explicitly allowing the current thread to be returned to the thread pool while the asynchronous operation is in progress. When the awaited operation completes, the continuation may run on a different thread. From the perspective of the runtime, this is normal and desirable behavior, but from the perspective of a Mutex, it is dangerous. If a thread acquires a mutex and then awaits, the continuation might resume on a different thread, and that different thread will attempt to release a mutex it does not own. This violates the mutex ownership rules and can result in runtime exceptions or undefined behavior.
			 * 
			 * Even if you imagine a scenario where the continuation resumes on the same thread by coincidence, there is a second, more serious problem. While the original thread is awaiting, it is effectively idle from the mutex’s point of view but still holds the lock. Any other thread attempting to acquire the mutex will block, even though the protected code is not actively executing. This creates a situation where progress is artificially stalled, and under load this can lead to starvation, where requests queue up behind a mutex that is held by a suspended async flow.
			 * 
			 * Once the mutex is acquired, the code must remain strictly synchronous until the mutex is released, ensuring that the same thread owns the mutex for the entire critical section and that the mutex is held for the shortest predictable duration.
			 */
			using var mutex = new Mutex(false, resource);
			mutex.WaitOne();

			try
			{
				/**
				 * The mutex is combined with a cache-based time window to enforce the minimum order placement interval.
				 * 
				 * After acquiring the mutex, the code checks a cache key tied to the same customer identifier. If an order was placed recently, the method immediately returns an error instead of placing another order. If no recent order exists, it proceeds and then records the placement in the cache. The mutex ensures that this check and set sequence is atomic for a given customer. Without the mutex, two concurrent requests could both see the cache value as false and both proceed to place orders before either writes the true value.
				 */
				var cacheKey = _staticCacheManger.BuildKey(new ("GlideBuy.Order.With.Lock.{0}"), resource);
				cacheKey.CacheTimeMinute = _orderSettings.MinimumOrderPlacementIntervalMinute;

				/**
				 * Calling GetAwaiter().GetResult() also blocks the current thread until the task completes, but it behaves more like the await keyword in terms of exception propagation. If the task faults, the original exception is thrown directly, not wrapped in an AggregateException. This makes error handling, logging, and debugging more predictable and closer to what you would see in a fully async flow. For this reason, GetAwaiter().GetResult() is generally considered the “cleaner” synchronous wait when you must block.
				 */
				var exist = _staticCacheManger.TryGetOrLoadAsync(cacheKey, () => false).GetAwaiter().GetResult();

				if (exist)
				{
					result = new PlaceOrderResult();
					result.Errors.Add("Minimum order placement interval violeted.");
				}
				else
				{
					result = placeOrder(details).GetAwaiter().GetResult();

					if (result.Success)
					{
						_staticCacheManger.SetAsync(cacheKey, true).Wait();
					}
				}

			}
			finally
			{
				mutex.ReleaseMutex();
			}

			return result;
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
