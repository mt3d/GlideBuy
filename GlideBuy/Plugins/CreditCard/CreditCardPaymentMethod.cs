using GlideBuy.Plugins.CreditCard.Components.PaymentCreditCard;
using GlideBuy.Services.Payments;
using GlideBuy.Services.Plugins;

namespace GlideBuy.Plugin.Payments.CreditCard
{
	public class CreditCardPaymentMethod : BasePlugin, IPaymentMethod
	{
		public PaymentMethodType PaymentMethodType => throw new NotImplementedException();

		public CreditCardPaymentMethod()
		{
			// TODO: Temporary. Remove once the plugin system is in place.
			PluginDescriptor = new PluginDescriptor
			{
				FriendlyName = "Credit Card",
			};
		}

		public Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest)
		{
			throw new NotImplementedException();
		}

		public async Task<string> GetPaymentMethodDescriptionAsync()
		{
			return "Pay using credit card";
		}

		public Type GetPublicViewComponent()
		{
			return typeof(PaymentCreditCardViewComponent);
		}

		public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
		{
			throw new NotImplementedException();
		}
	}
}
