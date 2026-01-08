using GlideBuy.Web.Models.Common;

namespace GlideBuy.Web.Models.Checkout
{
	public class CheckoutBillingAddressModel
	{
		public IList<AddressModel> ExistingAddresses { get; set; } = new List<AddressModel>();

		public AddressModel BillingNewAddress { get; set; } = new AddressModel();

		public bool ShipToSameAddress { get; set; }
		public bool ShipToSameAddressAllowed { get; set; }

		// TODO: Add VAT Number.
	}
}
