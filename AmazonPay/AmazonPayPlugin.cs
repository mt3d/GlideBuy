using GlideBuy.Services.Payments;

namespace GlideBuy.Plugin.Payments.AmazonPay
{
	public class AmazonPayPlugin : IPaymentMethod
	{
		public Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest)
		{
			/**
			 * When you use a payment method that redirects to a third-party
			 * site (like PayPal Standard), there is no need to do any processing.
			 * The real processing happens after the redirection.
			*/
			return Task.FromResult(new ProcessPaymentResult());
		}

		public PaymentMethodType PaymentMethodType
		{
			// TODO: return Standard if we're checking out.

			get
			{
				return PaymentMethodType.Button;
			}
		}
	}
}
