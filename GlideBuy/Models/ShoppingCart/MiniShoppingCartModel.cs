namespace GlideBuy.Models.ShoppingCart
{
	public record MiniShoppingCartModel
	{
		public IList<ShoppingCartItemModel> Items { get; set; } = new List<ShoppingCartItemModel>();

		public record ShoppingCartItemModel
		{
			public int Id { get; set; }

			public int ProductId { get; set; }

			public string ProductName { get; set; }

			public int Quantity { get; set; }
		}
	}
}
