using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Controllers
{
	public class HomeController : Controller
	{
		public HomeController()
		{
		}

		public ViewResult Index()
		{
			return View();
		}
	}
}
