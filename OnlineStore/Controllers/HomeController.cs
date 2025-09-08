using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.Repositories;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Controllers
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
			=> View(new ProductListViewModel
			{
				Products = productRepository.Products
					.Where(p => category == null || p.Category == category)
					.OrderBy(p => p.ProductId)
					.Skip((productPage - 1) * PageSize)
					.Take(PageSize),
				PagingInfo = new PagingInfo
				{
					CurrentPage = productPage,
					ItemsPerPage = PageSize,
					TotalItems = category == null ? productRepository.Products.Count() : productRepository.Products.Where(e => e.Category == category).Count()
				},
				CurrentCategory = category
			});	
	}
}
