using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models
{
	public class StoreDbContext : DbContext
	{
		public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

		public DbSet<Product> Products => Set<Product>();
		public DbSet<Order> Orders => Set<Order>();
		public DbSet<Category> Categories => Set<Category>();
		public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
	}
}
