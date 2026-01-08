namespace GlideBuy.Services.Payments
{
	public class ProcessPaymentRequest
	{
		// public int StoreId { get; set; }

		// public int CustomerId { get; set; }

		/**
		 * Used when order is not saved yet (payment gateways that do not redirect
		 * a customer to a third-party URL).
		 * 
		 * TODO: Add a detailed explanation.
		 */
		public Guid OrderGuid { get; set; }

		public decimal OrderTotal { get; set; }

		public string PaymentMethodSystemName { get; set; }
	}
}
