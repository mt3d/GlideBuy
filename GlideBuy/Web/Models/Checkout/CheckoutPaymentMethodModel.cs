namespace GlideBuy.Web.Models.Checkout
{
	public record CheckoutPaymentMethodModel
	{
		public IList<PaymentMethodModel> PaymentMethods { get; set; } = new List<PaymentMethodModel>();

		public record PaymentMethodModel
		{
			// TODO: Explain
			public string PaymentMethodSystemName { get; set; }

			public string Name { get; set; }
		}
	}
}
