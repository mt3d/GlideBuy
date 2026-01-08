using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class OffcanvasShoppingCartViewComponent : ViewComponent
	{
		private readonly IShoppingCartModelsFactory _shoppingCartModelsFactory;

		public OffcanvasShoppingCartViewComponent(IShoppingCartModelsFactory shoppingCartModelsFactory)
		{
			_shoppingCartModelsFactory = shoppingCartModelsFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = await _shoppingCartModelsFactory.PrepareMiniShoppingCartModelAsync();

			return View(model);
		}
	}
}
