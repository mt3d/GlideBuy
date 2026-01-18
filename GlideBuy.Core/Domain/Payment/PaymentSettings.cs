using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Domain.Payment
{
	public class PaymentSettings : ISettings
	{
		/**
		 *  Gets or sets a interval (in seconds) to reuse the same order GUID during
		 *  an order placement for multiple payment attempts.
		 *  
		 *  Set to 0 to generate a new order GUID for each payment attempt
		 */
		public int RegenerateOrderGuidInterval { get; set; }
	}
}
