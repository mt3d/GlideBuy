namespace OnlineStore.Core.Domain.Orders
{
	public enum OrderStatus
	{
		/**
		 * The initial state of an order, right after the customer places it.
		 * The order has been created in the database, but payment has not yet been confirmed.
		 * Inventory may not yet be reduced.
		 * The system (or admin) is still waiting for payment or manual approval before processing.
		 * 
		 * Example: A customer adds items to the cart, checks out, and chooses “Bank transfer”
		 * — the order becomes Pending until the payment is received.
		 */
		Pending = 10,

		/**
		 * Indicates that the order is being handled. Payment has usually been confirmed,
		 * and now items are being packed or prepared for shipment. The store or warehouse
		 * is fulfilling the order. An in-progress state between pending and completion.
		 * 
		 * Example: The payment gateway confirmed the transaction — the store staff are
		 * preparing the shipment.
		 */
		Processing = 20,

		/**
		 * The order has been successfully fulfilled and closed. All steps (payment,
		 * shipment, delivery) are done, and the customer has received the goods/services.
		 */
		Complete = 30,

		/**
		 * The order has been cancelled either by:
		 * The customer (before shipment/payment),
		 * The system (e.g., payment failed),
		 * Or the administrator (e.g., stock unavailable).
		 * 
		 * Refund or ignore.
		 */
		Cancelled = 40
	}
}
