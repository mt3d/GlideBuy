using GlideBuy.Models;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class OrderTotalsViewComponent : ViewComponent
	{
		private readonly Cart _shoppingCartService;
		private readonly IShoppingCartModelsFactory _shoppingCartModelsFactory;

		public OrderTotalsViewComponent(
			Cart shoppingCartService,
			IShoppingCartModelsFactory shoppingCartModelsFactory)
		{
			_shoppingCartService = shoppingCartService;
			_shoppingCartModelsFactory = shoppingCartModelsFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync(bool isEditable)
		{
			var cart = _shoppingCartService.Lines;
			var model = await _shoppingCartModelsFactory.PrepareOrderTotalsModelAsync(cart, isEditable);
			return View(model);
		}
	}
}
