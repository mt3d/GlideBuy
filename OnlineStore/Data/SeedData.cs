using GlideBuy.Core.Domain.Customers;
using GlideBuy.Data.Seeding;
using GlideBuy.Models;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Data
{
	public class SeedData
	{
		public static async Task EnsurePopulated(IApplicationBuilder app)
		{
			StoreDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
			ILogger<MainSeeder> logger = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ILogger<MainSeeder>>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}

			var seeder = new MainSeeder(context, logger);
			await seeder.SeedCategoriesAsync();
			await seeder.SeedUrlRecordsAsync();
			await seeder.SeedProductsAsync();

			await InstallCustomersAndRolesAsync(context);
		}

		private static async Task InstallCustomersAndRolesAsync(StoreDbContext context)
		{
			var crRegistered = new CustomerRole
			{
				Name = "Registered",
				Active = true,
				IsSystemRole = true,
				SystemName = "Registered"
			};
			var crGuests = new CustomerRole
			{
				Name = "Guests",
				Active = true,
				IsSystemRole = true,
				SystemName = "Guests"
			};
			var crVendors = new CustomerRole
			{
				Name = "Vendors",
				Active = true,
				IsSystemRole = true,
				SystemName = "Vendors"
			};

			var customerRoles = new List<CustomerRole>
			{
				crRegistered,
				crGuests,
				crVendors
			};

			context.CustomerRoles.AddRange(customerRoles);
			await context.SaveChangesAsync();
		}
	}
}
