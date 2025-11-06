using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlideBuy.Data.Repositories;
using GlideBuy.Models;
using GlideBuy.Models.ViewModels;

namespace GlideBuy.Controllers
{
	public class HomeController : Controller
	{
		// TODO: define as readonly
		private ProductRepository productRepository;

		public int PageSize = 4;

		public HomeController(ProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}

		public ViewResult Index(string? category, int productPage = 1)
		{
			return View(new ProductListViewModel
			{
				Products = productRepository.Products.Include(p => p.Category)
					.Where(p => category == null || p.Category.Name == category)
					.OrderBy(p => p.ProductId)
					.Skip((productPage - 1) * PageSize)
					.Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = productPage,
					ItemsPerPage = PageSize,
					TotalItems = category == null
						? productRepository.Products.Count()
						: productRepository.Products.Include(p => p.Category).Where(e => e.Category.Name == category).Count()
				},
				CurrentCategory = category
			});
		}
	}
}
