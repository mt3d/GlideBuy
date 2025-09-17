using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Models.Repositories;

namespace OnlineStore.Components
{
	public class NavigationMenuViewComponent : ViewComponent
	{
		// private IStoreRepository repository;
		private ProductRepository repository;

		public NavigationMenuViewComponent(ProductRepository repo)
		{
			this.repository = repo;
		}

		public IViewComponentResult Invoke()
		{
			ViewBag.SelectedCategory = RouteData?.Values["category"];

			// return View(repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
			// Operation: Get all distinct categories in ascending order
			// TODO: Use CategoryService (GetAllCategories)
			return View(repository.Products
				.Include(p => p.Category)
				.Select(x => x.Category.Name)
				.Distinct().OrderBy(x => x));
		}
	}
}
