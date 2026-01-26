namespace GlideBuy.Models.ShoppingCart
{
	public class OrderTotalsModel
	{
		public bool IsEditable { get; set; }

		public int NumberOfItems { get; set; }

		public string Subtotal { get; set; }

		public string SubtotalDiscount { get; set; }

		public string ShippingStatus { get; set; }

		public bool RequiresShipping { get; set; }

		public string SelectedShippingMethod { get; set; }

		public bool HideShippingTotal { get; set; }

		public string Tax { get; set; }

		public string Total { get; set; }
	}
}
