namespace GlideBuy.Services.Payments
{
	/**
	 * Original name: ProcessPaymentRequest
	 * 
	 * At its core, it is a stateful carrier of payment-related checkout information that is passed across multiple layers and multiple moments in time during checkout. It exists specifically to decouple payment logic from controller state, HTTP requests, and UI forms, and to provide a stable object that payment plugins can rely on regardless of how the checkout flow is structured.
	 * 
	 * It is a long-lived payment context object that starts being populated during checkout, is persisted temporarily by the order processing service, is consumed by the payment plugin during ProcessPayment, and may later be reused during post-processing or recurring billing.
	 */
	public class OrderPaymentContext
	{
		public OrderPaymentContext()
		{
			/**
			 * This GUID is not the database order ID. Instead, it is a pre-order identifier that exists before the order entity is saved. Its primary purpose is idempotency and correlation. Payment gateways that do not redirect the customer can safely associate their internal transactions with this GUID, and NopCommerce can later match the payment attempt to the eventual order. The timestamp adds a security dimension, allowing the platform to invalidate or reason about stale payment attempts.
			 */
			OrderGuid = Guid.NewGuid();
			OrderGuidGeneratedOnUtc = DateTime.UtcNow;
		}

		public Guid OrderGuid { get; set; }

		public DateTime? OrderGuidGeneratedOnUtc { get; set; }

		/**
		 * The basic properties such as StoreId, CustomerId, and OrderTotal establish the transactional context. These values anchor the payment attempt to a specific store and customer and ensure that payment plugins do not need to re-query cart totals or store configuration during execution. This is intentional: payment plugins are expected to operate on immutable input, not on live cart state that could change mid-request.
		 */
		public int CustomerId { get; set; }

		public decimal OrderTotal { get; set; }

		/**
		 * The PaymentMethodSystemName is particularly important. It is the logical key that selects which payment plugin will execute. This string is set earlier in the checkout flow when the customer chooses a payment method and is persisted on the customer as a generic attribute. By copying it into the ProcessPaymentRequest, the order placement pipeline ensures that the payment method cannot silently change between steps.
		 */
		public string PaymentMethodSystemName { get; set; }


		/**
		 * The recurring payment section further demonstrates that this class is not just about “processing a payment” in isolation. For recurring products, the ProcessPaymentRequest also acts as a bridge between the initial order and future automated payments. The InitialOrder property links child recurring payments back to their parent order, while the cycle length, period, and total cycles define the billing schedule. This allows the same payment pipeline to be reused for both initial and recurring charges, with the request object carrying the necessary metadata.
		 */

		/**
		 * Payment plugins can store arbitrary key-value pairs here, such as transaction tokens, session identifiers, redirect URLs, or gateway-specific flags. These values can survive across checkout steps and even across requests, depending on how the request object is stored. This is what enables complex workflows like 3D Secure, hosted checkout pages, or delayed captures without forcing changes to the core domain model.
		 */
	}
}
