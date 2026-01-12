using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Areas.Admin.Controllers
{
	public class PluginController : Controller
	{
		[Area("Admin")]
		public IActionResult List()
		{
			return View();
		}
	}
}
