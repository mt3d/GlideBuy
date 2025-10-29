using Microsoft.AspNetCore.Mvc;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Data.Repositories;
using GlideBuy.Models;

namespace GlideBuy.Controllers
{
	public class OrderController : Controller
	{
		private OrderRepository repository;
		private Cart cart;

		public OrderController(OrderRepository repositoryService, Cart cartService)
		{
			this.repository = repositoryService;
			this.cart = cartService;
		}

		public ViewResult Checkout() => View(new Order());

		[HttpPost]
		public IActionResult Checkout(Order order)
		{
			if (cart.Lines.Count() == 0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty!");
			}
			if (ModelState.IsValid)
			{
				order.Lines = cart.Lines.ToArray();
				repository.Save(order);
				cart.Clear();
				return RedirectToPage("/Completed", new { orderId = order.OrderId });
			} else
			{
				return View();
			}
		}
	}
}
