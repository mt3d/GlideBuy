using GlideBuy.Models;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class OrderTotalsViewComponent : ViewComponent
	{
		private readonly IShoppingCartService _shoppingCartService;
		private readonly IShoppingCartModelsFactory _shoppingCartModelsFactory;

		public OrderTotalsViewComponent(
			IShoppingCartService shoppingCartService,
			IShoppingCartModelsFactory shoppingCartModelsFactory)
		{
			_shoppingCartService = shoppingCartService;
			_shoppingCartModelsFactory = shoppingCartModelsFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync(bool isEditable)
		{
			var cart = await _shoppingCartService.GetShoppingCartAsync();

			var model = await _shoppingCartModelsFactory.PrepareOrderTotalsModelAsync(cart, isEditable);
			return View(model);
		}
	}
}
