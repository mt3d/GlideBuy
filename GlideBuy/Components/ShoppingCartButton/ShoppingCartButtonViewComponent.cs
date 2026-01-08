using GlideBuy.Web.Factories;
using GlideBuy.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components.ShoppingCartButton
{
	public class ShoppingCartButtonViewComponent : ViewComponent
	{
		private readonly ICommonModelFactory _commonModelFactory;

		public ShoppingCartButtonViewComponent(ICommonModelFactory commonModelFactory)
		{
			_commonModelFactory = commonModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = await _commonModelFactory.PrepareCartButtonModelAsync();

			return View(model);
		}
	}
}
