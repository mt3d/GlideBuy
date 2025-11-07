namespace GlideBuy.Web.Models.Checkout
{
	public class OnePageCheckoutModel
	{
		public bool ShippingRequired { get; set; }

		public bool DisableBillingAddressCheckoutStep { get; set; }

		public bool DisplayCaptcha { get; set; }
	}
}
