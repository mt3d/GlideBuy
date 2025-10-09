namespace OnlineStore.Models.ShoppingCart
{
	public class ShoppingCartModel
	{
		public string ReturnUrl { get; set; } = "/";

		public Cart? Cart { get; set; }
	}
}
