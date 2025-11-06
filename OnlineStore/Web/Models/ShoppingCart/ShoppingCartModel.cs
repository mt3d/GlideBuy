using GlideBuy.Models;

namespace GlideBuy.Web.Models.ShoppingCart
{
	public class ShoppingCartModel
	{
		public ShoppingCartModel()
		{
			Items = new List<ShoppingCartItemModel>();
			OrderReviewData = new OrderReviewDataModel();
			Warnings = new List<string>();
		}

		public string ReturnUrl { get; set; } = "/";

		//public Cart? Cart { get; set; }

		public bool ShowSku { get; set; }

		public bool ShowProductImages { get; set; }

		public bool IsEditable { get; set; }

		public bool IsReadyToCheckout { get; set; }

		public bool ShowVendorName { get; set; }

		public IList<ShoppingCartItemModel> Items { get; set; }

		public OrderReviewDataModel OrderReviewData { get; set; }

		public IList<string> Warnings { get; set; }

		public string MinOrderSubtotalWarning { get; set; }

		public bool HasTermsOfServiceOnCartPage { get; set; }

		public bool HasTermsOfServiceOnOrderConfirmPage { get; set; }

		public bool HasTermsOfServicePopup { get; set; }

		public class ShoppingCartItemModel
		{
			public string Sku { get; set; }

			public long ProductId { get; set; }

			public string ProductName { get; set; }

			public int Quantity { get; set; }

			public string UnitPrice { get; set; }
			public decimal UnitPriceValue { get; set; }

			public string Subtotal { get; set; }
			public decimal SubtotalValue { get; set; }
		}

		public class OrderReviewDataModel
		{
			public bool Display { get; set; }

			public bool IsShippable { get; set; }

			public bool SelectedPickupInStore { get; set; }

			public string ShippingMethod { get; set; }

			public string PaymentMethod { get; set; }
		}
	}
}
