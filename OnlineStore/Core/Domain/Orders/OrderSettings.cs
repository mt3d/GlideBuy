using GlideBuy.Core.Configuration;

namespace GlideBuy.Core.Domain.Orders
{
	public class OrderSettings : ISettings
	{
		/// <summary>
		/// Gets or sets a minimum subtotal amount for orders.
		/// </summary>
		public decimal MinOrderSubtotalAmount { get; set; }

		/// <summary>
		/// Gets or sets a minimum total amount for orders.
		/// </summary>
		public decimal MinOrderTotalAmount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether "One-Page Checkout" is enabled.
		/// </summary>
		public bool OnePageCheckoutEnabled { get; set; }
	}
}
