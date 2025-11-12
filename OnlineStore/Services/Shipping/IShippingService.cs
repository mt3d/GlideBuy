using GlideBuy.Models;

namespace GlideBuy.Services.Shipping
{
	public interface IShippingService
	{
		bool IsShippingEnabled(ShoppingCartItem item);
	}
}
