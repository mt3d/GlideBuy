using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Domain.Seo;
using GlideBuy.Models;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Data
{
	public class SeedData
	{
		public static async Task EnsurePopulated(IApplicationBuilder app)
		{
			StoreDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}

			Category[] categories =
			{
				new Category
					{
						Name = "Smartphone"
					},
					new Category
					{
						Name = "Smartwatch"
					},
					new Category
					{
						Name = "Earbuds"
					},
					new Category
					{
						Name = "Laptop"
					},
					new Category
					{
						Name = "E-Reader"
					},
					new Category
					{
						Name = "Accessories"
					},
					new Category
					{
						Name = "Smarthome"
					}
			};

			if (!context.Categories.Any())
			{
				context.Categories.AddRange(categories);
			}

			if (!context.UrlRecords.Any())
			{
				context.UrlRecords.Add(new UrlRecord { EntityId = 1, EntityName = "Category", IsActive = true, Slug = "smartphone" });
				context.UrlRecords.Add(new UrlRecord { EntityId = 2, EntityName = "Category", IsActive = true, Slug = "smartwatch" });
				context.UrlRecords.Add(new UrlRecord { EntityId = 3, EntityName = "Category", IsActive = true, Slug = "earbuds" });
				context.UrlRecords.Add(new UrlRecord { EntityId = 4, EntityName = "Category", IsActive = true, Slug = "laptops" });
				context.UrlRecords.Add(new UrlRecord { EntityId = 5, EntityName = "Category", IsActive = true, Slug = "e-reader" });
				context.UrlRecords.Add(new UrlRecord { EntityId = 6, EntityName = "Category", IsActive = true, Slug = "accessories" });
				context.UrlRecords.Add(new UrlRecord { EntityId = 7, EntityName = "Category", IsActive = true, Slug = "smarthome" });
			}
			
			if (!context.Products.Any())
			{
				context.Products.AddRange(
				new Product
				{
					Name = "iPhone 15 Pro",
					ShortDescription = "Apple's latest flagship smartphone with A17 Pro chip and titanium design.",
					Category = categories[0],
					Price = 999,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Samsung Galaxy S24 Ultra",
					ShortDescription = "High-end Android phone with 200MP camera and S Pen support.",
					Category = categories[0],
					Price = 1199,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Google Pixel 8 Pro",
					ShortDescription = "Google’s premium phone with Tensor G3 chip and advanced AI features.",
					Category = categories[0],
					Price = 999,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 8
				},
				new Product
				{
					Name = "OnePlus 12",
					ShortDescription = "Flagship killer with Snapdragon 8 Gen 3 and ultra-fast charging.",
					Category = categories[0],
					Price = 799,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Apple Watch Series 9",
					ShortDescription = "Smartwatch with S9 chip, double tap gesture, and improved Siri.",
					Category = categories[1],
					Price = 399,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Samsung Galaxy Watch 6",
					ShortDescription = "Wear OS-powered smartwatch with health tracking and sleek design.",
					Category = categories[1],
					Price = 329,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Fitbit Versa 4",
					ShortDescription = "Fitness-focused smartwatch with heart rate monitoring and sleep tracking.",
					Category = categories[1],
					Price = 229,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Garmin Venu 3",
					ShortDescription = "Premium smartwatch with GPS, health monitoring, and AMOLED display.",
					Category = categories[1],
					Price = 449,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 7
				},
				new Product
				{
					Name = "Sony WF-1000XM5",
					ShortDescription = "Noise-canceling wireless earbuds with superior sound quality.",
					Category = categories[2],
					Price = 299,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Apple AirPods Pro (2nd Gen)",
					ShortDescription = "Wireless earbuds with active noise cancellation and MagSafe case.",
					Category = categories[2],
					Price = 249,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 6
				},
				new Product
				{
					Name = "Samsung Galaxy Buds2 Pro",
					ShortDescription = "Hi-Fi wireless earbuds with ANC and 360 Audio.",
					Category = categories[2],
					Price = 229,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Nothing Ear (2)",
					ShortDescription = "Stylish wireless earbuds with great audio and unique transparent design.",
					Category = categories[2],
					Price = 149,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 5
				},
				new Product
				{
					Name = "Dell XPS 13 (2024)",
					ShortDescription = "Compact and powerful Windows ultrabook with Intel Core Ultra processor.",
					Category = categories[3],
					Price = 1199,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "MacBook Air 15\" (M2, 2023)",
					ShortDescription = "Slim and lightweight laptop with Apple's M2 chip and 18-hour battery.",
					Category = categories[3],
					Price = 1299,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "ASUS ROG Zephyrus G14",
					ShortDescription = "Powerful gaming laptop with AMD Ryzen 9 and RTX 4060 GPU.",
					Category = categories[3],
					Price = 1599,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 4
				},
				new Product
				{
					Name = "Lenovo Yoga 9i",
					ShortDescription = "2-in-1 premium laptop with rotating soundbar and 4K OLED display.",
					Category = categories[3],
					Price = 1399,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Amazon Kindle Paperwhite (11th Gen)",
					ShortDescription = "E-reader with 6.8\" display, adjustable warm light, and weeks-long battery.",
					Category = categories[4],
					Price = 139,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 3
				},
				new Product
				{
					Name = "Logitech MX Master 3S",
					ShortDescription = "High-performance wireless mouse with ultra-quiet clicks and ergonomic design.",
					Category = categories[5],
					Price = 99,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 2
				},
				new Product
				{
					Name = "Anker 737 Power Bank (PowerCore 24K)",
					ShortDescription = "Portable charger with 140W output and fast-charging support.",
					Category = categories[5],
					Price = 159,
					Published = true,
					Deleted = false,
				},
				new Product
				{
					Name = "Google Nest Hub (2nd Gen)",
					ShortDescription = "Smart display with Google Assistant and sleep sensing.",
					Category = categories[6],
					Price = 99,
					Published = true,
					Deleted = false,
					ShowOnHomePage = true,
					DisplayOrder = 1
				}
				);
				context.SaveChanges();
			}

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
