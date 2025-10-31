using GlideBuy.Core.Domain.Configuration;
using GlideBuy.Core.Domain.Orders;
using GlideBuy.Models;
using GlideBuy.Models.Localization;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Data
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

		public DbSet<Setting> Settings => Set<Setting>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<Order>()
				.Property(e => e.OrderStatus)
				.HasConversion<int>();

			modelBuilder
				.Entity<Order>()
				.Property(e => e.ShippingStatus)
				.HasConversion<int>();

			modelBuilder
				.Entity<Order>()
				.Property(e => e.PaymentStatus)
				.HasConversion<int>();

			modelBuilder
				.Entity<Product>()
				.Property(e => e.InventoryManagementMethod)
				.HasConversion<int>();
		}
	}
}
