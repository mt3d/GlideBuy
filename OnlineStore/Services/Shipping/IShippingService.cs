using GlideBuy.Core.Domain.Orders;

namespace GlideBuy.Services.Shipping
{
	public interface IShippingService
	{
		bool IsShippingEnabled(ShoppingCartItem item);
	}
}
