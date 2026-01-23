using GlideBuy.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Controllers
{
	public class CustomerController : Controller
	{

		public async Task<IActionResult> Register(string returnUrl)
		{
			// TODO: Check if user registration is disabled.

			var model = new RegisterModel();

			return View();
		}
	}
}
