namespace OnlineStore.Services.Payments
{
	public interface IPaymentMethod
	{
		Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest);
	}
}
