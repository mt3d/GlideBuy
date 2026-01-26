namespace GlideBuy.Models.Checkout
{
	public record CheckoutShippingMethodModel
	{
		public IList<ShippingMethodModel> ShippingMethods { get; set; } = new List<ShippingMethodModel>();

		public record ShippingMethodModel
		{
			public string Name { get; set; }
			public string Description { get; set; }

			public int DisplayOrder { get; set; }

			public bool Selected { get; set; }
		}
	}
}
