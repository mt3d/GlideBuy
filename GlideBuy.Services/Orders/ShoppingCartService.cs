using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models;
using GlideBuy.Services.Shipping;
using Microsoft.AspNetCore.Http;

namespace GlideBuy.Services.Orders
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IHttpContextAccessor httpContextAccessor;
		private ISession? Session { get; set; }
		private readonly IShippingService _shippingService;

		public ShoppingCartService(
			IHttpContextAccessor httpContextAccessor,
			IShippingService shippingService)
		{
			this.httpContextAccessor = httpContextAccessor;
			_shippingService = shippingService;
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

		public async Task<bool> ShoppingCartRequiresShippingAsync(IList<ShoppingCartItem> cart)
		{
			return cart.Any(shoppingCartItem =>
			{
				return _shippingService.IsShippingEnabled(shoppingCartItem);
			});
		}
	}
}
