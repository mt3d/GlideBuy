namespace GlideBuy.Models.Checkout
{
	public class OnePageCheckoutModel
	{
		public bool ShippingRequired { get; set; }

		public bool DisableBillingAddressCheckoutStep { get; set; }

		public bool DisplayCaptcha { get; set; }

		public CheckoutBillingAddressModel BillingAddress { get; set; } = new CheckoutBillingAddressModel();
	}
}
