namespace GlideBuy.Models.ShoppingCart
{
	public class OrderSummaryModel
	{
		public bool IsCartPage { get; set; }

		public string MinOrderSubtotalWarning { get; set; } = string.Empty;

		public bool HasTermsOfServiceOnCartPage { get; set; }
	}
}
