using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class CategoriesMenuViewComponent : ViewComponent
	{
		private readonly ICatalogModelFactory _catalogModelFactory;

		public CategoriesMenuViewComponent(ICatalogModelFactory catalogModelFactory)
		{
			_catalogModelFactory = catalogModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync(bool isStatic)
		{
			ViewBag.IsStatic = isStatic;

			var model = await _catalogModelFactory.PrepareCategoriesMegaMenuModelAsync();

			return View(model);
		}
	}
}
