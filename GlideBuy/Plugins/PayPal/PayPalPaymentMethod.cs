using GlideBuy.Services.Payments;
using GlideBuy.Services.Plugins;

namespace GlideBuy.Plugins.PayPal
{
	public class PayPalPaymentMethod : BasePlugin, IPaymentMethod
	{
		public PaymentMethodType PaymentMethodType => throw new NotImplementedException();

		public Task<OrderPaymentContext> GetPaymentInfoAsync(IFormCollection form)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetPaymentMethodDescriptionAsync()
		{
			throw new NotImplementedException();
		}

		public Type GetPublicViewComponent()
		{
			throw new NotImplementedException();
		}

		public Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest)
		{
			throw new NotImplementedException();
		}

		public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
		{
			throw new NotImplementedException();
		}
	}
}
