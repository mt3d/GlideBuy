namespace GlideBuy.Services.Payments
{
	public interface IPaymentMethod
	{
		Task<ProcessPaymentResult> ProcessPaymentAsync(OrderPaymentContext processPaymentRequest);

		PaymentMethodType PaymentMethodType { get; }
	}
}
