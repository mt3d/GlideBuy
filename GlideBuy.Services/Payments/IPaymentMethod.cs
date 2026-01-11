using GlideBuy.Services.Plugins;

namespace GlideBuy.Services.Payments
{
	public interface IPaymentMethod : IPlugin
	{
		Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest);

		PaymentMethodType PaymentMethodType { get; }
	}
}
