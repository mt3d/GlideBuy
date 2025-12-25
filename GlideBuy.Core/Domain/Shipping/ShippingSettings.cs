using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Domain.Shipping
{
	public class ShippingSettings : ISettings
	{
		/// <summary>
		/// Indicates whether "Ship to the same address" option is enabled.
		/// </summary>
		public bool ShipToSameAddress { get; set; }

		public bool AllowPickupInStore { get; set; }

		public bool FreeShippingOverXEnabled { get; set; }

		public decimal FreeShippingOverXValue { get; set; }
	}
}
