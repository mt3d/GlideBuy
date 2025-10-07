using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Domain.Orders;

namespace OnlineStore.Data.Repositories
{
	public class OrderRepository
	{
		private StoreDbContext context;

		public OrderRepository(StoreDbContext context)
		{
			this.context = context;
		}

		public IQueryable<Order> Orders => context.Orders
			.Include(o => o.Lines)
			.ThenInclude(l => l.Product);

		public void Save(Order order)
		{
			// Tells EF that the entitites exist in the database.
			context.AttachRange(order.Lines.Select(l => l.Product));

			if (order.OrderId == 0)
			{
				context.Orders.Add(order);
			}
			context.SaveChanges();
		}
	}
}
