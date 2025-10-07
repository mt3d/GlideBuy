namespace OnlineStore.Core.Domain.Payment
{
	public enum PaymentStatus
	{
		/**
		 * Initial payment state. The customer has placed the order, but no payment
		 * action has been confirmed yet. This can happen when:
		 * The customer selected an offline payment method (e.g. bank transfer, cash on delivery).
		 * The payment gateway hasn’t responded yet.
		 * The store is still waiting for authorization.
		 */
		Pending = 10,

		/**
		 * The payment has been authorized by the payment gateway, but not yet captured
		 * (i.e., not fully charged). It means the funds are reserved on the customer’s card,
		 * but not transferred to the merchant.
		 * 
		 * This status is common in gateways that support two-step payments:
		 * Authorization (reserve funds)
		 * Capture (charge later)
		 * 
		 * Example: A credit card payment is authorized for $100, but the merchant will
		 * capture it only when the order ships.
		 */
		Authorized = 20,

		/**
		 * This is the “successful payment” state.
		 */
		Paid = 30,

		/**
		 * A partial refund has been issued. For example, one item in a multi-item order
		 * was returned, so part of the payment was refunded.
		 */
		PartiallyRefunded = 50,

		/**
		 * The order may still exist in records, but financially it’s settled at zero.
		 */
		Refunded = 40,

		/**
		 * The payment authorization was voided before it was captured. This applies only
		 * to authorized but not charged payments. No money was actually taken — the authorization
		 * was simply canceled.
		 * 
		 * Example: The merchant decided not to fulfill the order, so they voided
		 * the authorization before capture.
		 */
		Voided = 50
	}
}
