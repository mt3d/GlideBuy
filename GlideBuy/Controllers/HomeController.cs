using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlideBuy.Data.Repositories;
using GlideBuy.Models;
using GlideBuy.Models.ViewModels;

namespace GlideBuy.Controllers
{
	public class HomeController : Controller
	{
		private ProductRepository productRepository;

		public int PageSize = 4;

		public HomeController(ProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		public ViewResult Index(/*string? category, int productPage = 1*/)
		{
			return View();
		}
	}
}
