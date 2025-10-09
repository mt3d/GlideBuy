using OnlineStore.Models;

namespace GlideBuy.Web.Models.ShoppingCart
{
	public class ShoppingCartModel
	{
		public ShoppingCartModel()
		{
			Items = new List<ShoppingCartItemModel>();
			OrderReviewData = new OrderReviewDataModel();
		}

		public string ReturnUrl { get; set; } = "/";

		public Cart? Cart { get; set; }

		public IList<ShoppingCartItemModel> Items { get; set; }

		public OrderReviewDataModel OrderReviewData { get; set; }

		public class ShoppingCartItemModel
		{
			public string Sku { get; set; }

			public string ProductName { get; set; }
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
