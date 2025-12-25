using GlideBuy.Core.Domain.Orders;
using GlideBuy.Services.ProductCatalog;

namespace GlideBuy.Services.Shipping
{
	public class ShippingService : IShippingService
	{
		private readonly IProductService _productService;

		public ShippingService(IProductService productService)
		{
			_productService = productService;
		}

		public bool IsShippingEnabled(ShoppingCartItem item)
		{
			if (item.Product != null)
			{
				return item.Product.IsShippingEnabled;

				// await _productService.GetProductByIdAsync(item.ProductId)
			}

			// TODO: Handle attributes and related products.

			return false;
		}
	}
}
