namespace GlideBuy.Services.Payments
{
	public enum PaymentMethodType
	{
		/**
		 * All payment information is entered directly on your store’s website.
		 * The payment plugin handles card info (or other credentials) without leaving your site.
		 * 
		 * Cons:
		 * You are responsible for PCI DSS compliance if handling card data.
		 */
		Standard = 10,

		/**
		 * The customer is redirected to a third-party site to complete payment.
		 * Your site doesn’t collect payment info; the external provider handles it.
		 * 
		 * Provider returns the customer to your site (callback/return URL).
		 * You must handle “return URL” and verify payment completion.
		 */
		Redirection = 20,

		/**
		 * A payment button is embedded on your site or rendered via a script, but
		 * the actual processing may happen off-site or via JS API.
		 * 
		 * Customer sees a payment button (e.g., “Pay with PayPal”).
		 * Slightly more complex integration than simple redirection.
		 */
		Button = 30
	}
}
