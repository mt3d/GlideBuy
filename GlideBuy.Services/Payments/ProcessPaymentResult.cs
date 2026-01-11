using GlideBuy.Core.Domain.Payment;

namespace GlideBuy.Services.Payments
{
	/**
	 * The outcome of a payment attempt after communicating with a payment gateway.
	 */
	public class ProcessPaymentResult
	{
		public ProcessPaymentResult()
		{
			Errors = new List<string>();
		}

		public bool Success => !Errors.Any();

		public void AddErrors(string error)
		{
			Errors.Add(error);
		}

		public IList<string> Errors { get; set; }

		/**
		 * Address Verification Service result.
		 * 
		 * Many credit card gateways check whether the billing address matches what
		 * the card issuer has on file.
		 * 
		 * Common values (from Visa/MasterCard systems):
		 * Y – Address and ZIP match.
		 * N – No match.
		 * A – Address matches, ZIP doesn’t.
		 * Z – ZIP matches, address doesn’t.
		 * 
		 * Used mainly for fraud detection or compliance.
		 */
		public string AvsResult { get; set; }

		/**
		 * Result of verifying the CVV2 security code (the 3 or 4 digits on the card).
		 * 
		 * Typical codes:
		 * M – Match.
		 * N – No match.
		 * P – Not processed.
		 * S – Should have been present.
		 * U – Issuer unable to process.
		 */
		public string Cvv2Result { get; set; }

		/**
		 * Unique identifier assigned by the payment gateway when the payment is authorized.
		 * You use it later to capture, void, or refund the transaction.
		 * Each authorization has a distinct ID (like a reference number).
		 */
		public string AuthorizationTransactionId { get; set; }

		/**
		 * Another identifier, often shorter, used by some gateways.
		 * It can represent an internal code from the payment processor (less standardized
		 * than AuthorizationTransactionId).
		 */
		public string AuthorizationTransactionCode { get; set; }

		/**
		 * The raw result or message from the authorization attempt.
		 * Contains the approval/decline message from the payment processor.
		 */
		public string AuthorizationTransactionResult { get; set; }

		public PaymentStatus NewPaymentStatus { get; set; } = PaymentStatus.Pending;
	}
}
