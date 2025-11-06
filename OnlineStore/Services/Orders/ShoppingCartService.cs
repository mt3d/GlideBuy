using GlideBuy.Infrastructure;
using GlideBuy.Models;

namespace GlideBuy.Services.Orders
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IHttpContextAccessor httpContextAccessor;
		private ISession? Session { get; set; }

		public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		private Cart GetCart()
		{
			ISession? session = httpContextAccessor.HttpContext?.Session;
			Cart cart = session?.GetJson<Cart>("Cart") ?? new Cart();
			Session = session;

			return cart;
		}

		public async Task<List<ShoppingCartItem>> GetShoppingCartAsync()
		{
			return GetCart().Lines;
		}

		public void AddToCartAsync(Product product, int quantity)
		{
			var cart = GetCart();
			cart.AddItem(product, quantity);

			Session?.SetJson("Cart", cart);
		}

		public void DeleteShoppingCartItemAsync(Product product)
		{
			var cart = GetCart();
			cart.RemoveLine(product);

			Session?.SetJson("Cart", cart);
		}

		public void ClearShoppingCartAsync()
		{
			Session?.Remove("Cart");
		}
	}
}
