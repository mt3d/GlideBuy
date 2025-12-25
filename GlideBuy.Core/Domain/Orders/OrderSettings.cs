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
		public bool OnePageCheckoutEnabled { get; set; } = true;

		public bool HasTermsOfServiceOnCartPage { get; set; }

		public bool HasTermsOfServiceOnOrderConfirmPage { get; set; }

		/// <summary>
		/// This setting exists for several practical operational, maintenance,
		/// and business scenarios.
		/// 
		/// A common reason for disabling checkout is to temporarily stop new orders
		/// while the store is undergoing maintenance, data migration, or backend updates.
		/// For example:
		/// The merchant is updating shipping or tax configurations.
		/// Payment gateways are being reconfigured or replaced.
		/// Inventory data is being synchronized with an ERP system.
		/// A new checkout plugin is being tested.
		/// 
		/// Some businesses use NopCommerce purely as a product catalog, not as a
		/// transactional store.
		/// 
		/// In certain circumstances, a store may need to pause checkout due to legal
		/// or regulatory issues.
		/// 
		/// If a store experiences severe inventory shortages, delivery delays, or
		/// supplier issues, the owner may choose to freeze checkout temporarily.
		/// 
		/// Developers and QA testers often deploy NopCommerce in staging or demo environments.
		/// Checkout is disabled there to:
		/// Avoid processing real payments.
		/// Prevent fake orders from reaching production systems.
		/// Allow testing browsing, cart, and UI without transactional side effects.
		/// 
		/// In B2B contexts, some merchants may require customers to submit purchase
		/// requests that are reviewed and approved manually before an order is finalized.
		/// </summary>
		public bool CheckoutDisabled { get; set; }

		public bool AnonymousCheckoutDisabled { get; set; }

		public bool DisableBillingAddressCheckoutStep { get; set; }
	}
}
