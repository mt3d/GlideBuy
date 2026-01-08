using GlideBuy.Models.Common;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Models.Common;

namespace GlideBuy.Web.Factories
{
	public class CommonModelFactory : ICommonModelFactory
	{
		private IShoppingCartService _shoppingCartService;

		public CommonModelFactory(IShoppingCartService shoppingCartService)
		{
			_shoppingCartService = shoppingCartService;
		}

		public async Task<LogoModel> PrepareLogoModelAsync()
		{
			// TODO: Get the current store, then get the localized name of it.

			var model = new LogoModel()
			{
				StoreName = "GlideBuy",
				LogoPath = "/images/logo.png"
			};

			return model;
		}

		public async Task<ShoppingCartButtonModel> PrepareCartButtonModelAsync()
		{
			var model = new ShoppingCartButtonModel();

			// TODO: Read current customer.
			if (true)
			{
				model.ShoppingCartItems = (await _shoppingCartService.GetShoppingCartAsync()).Sum(item => item.Quantity);
			}

			return model;
		}
	}
}
