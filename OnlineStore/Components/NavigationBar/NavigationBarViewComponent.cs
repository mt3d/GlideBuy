using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components.NavigationBar
{
	public class NavigationBarViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(bool isCategoriesMenuStatic)
		{
			ViewBag.IsCategoriesMenuStatic = isCategoriesMenuStatic;
			Console.WriteLine(ViewBag.IsCategoriesMenuStatic);

			return View();
		}
	}
}
