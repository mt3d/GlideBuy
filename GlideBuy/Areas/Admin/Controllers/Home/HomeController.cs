using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Areas.Admin.Controllers.Home
{
	// TODO: Use a base controller for all admin controllers
	// should be authorized.
	[Area("Admin")]
	public class HomeController : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
