using GlideBuy.Services.Plugins;
using Microsoft.AspNetCore.Http;

namespace GlideBuy.Services.Payments
{
	public interface IPaymentMethod : IPlugin
	{
		Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest);

		PaymentMethodType PaymentMethodType { get; }

		Task<string> GetPaymentMethodDescriptionAsync();

		Type? GetPublicViewComponent();
		
		// Returns a list of warnings.
		Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form);

		Task<OrderPaymentContext> GetPaymentInfoAsync(IFormCollection form);
	}
}
