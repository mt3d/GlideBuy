using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Components
{
	public class CategoriesMenuViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(bool isStatic)
		{
			Console.WriteLine(isStatic);

			ViewBag.IsStatic = isStatic;
			return View();
		}
	}
}
