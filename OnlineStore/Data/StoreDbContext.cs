using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Models.Localization;

namespace OnlineStore.Data
{
	public class StoreDbContext : DbContext
	{
		public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

		public DbSet<Product> Products => Set<Product>();
		public DbSet<Order> Orders => Set<Order>();
		public DbSet<Category> Categories => Set<Category>();
		public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

		public DbSet<Language> Languages => Set<Language>();
		public DbSet<LocalizationResource> LocalizationResources => Set<LocalizationResource>();
	}
}
