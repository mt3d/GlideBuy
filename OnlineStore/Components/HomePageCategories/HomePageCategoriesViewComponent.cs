using GlideBuy.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class HomePageCategoriesViewComponent : ViewComponent
	{
		private readonly ICatalogModelFactory _catalogModelFactory;

		public HomePageCategoriesViewComponent(ICatalogModelFactory catalogModelFactory)
		{
			_catalogModelFactory = catalogModelFactory;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var model = await _catalogModelFactory.PrepareHomePageCategoryModelsAsync();

			if (!model.Any())
			{
				return Content("");
			}

			return View(model);
		}
	}
}
