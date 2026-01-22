using GlideBuy.Services.Payments;
using GlideBuy.Services.Plugins;

namespace GlideBuy.Plugins.PayPal
{
	public class PayPalPaymentMethod : BasePlugin, IPaymentMethod
	{
		public PayPalPaymentMethod()
		{
			// TODO: Temporary. Remove once the plugin system is in place.
			PluginDescriptor = new PluginDescriptor
			{
				FriendlyName = "PayPal",
				SystemName = "PayPal"
			};
		}


		public PaymentMethodType PaymentMethodType => throw new NotImplementedException();

		public Task<OrderPaymentContext> GetPaymentInfoAsync(IFormCollection form)
		{
			return Task.FromResult(new OrderPaymentContext());
		}

		public async Task<string> GetPaymentMethodDescriptionAsync()
		{
			return "Pay using PayPal";
		}

		public Type? GetPublicViewComponent()
		{
			return null;
		}

		public Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest)
		{
			throw new NotImplementedException();
		}

		public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
		{
			return Task.FromResult<IList<string>>(new List<string>());
		}
	}
}
