using CsvHelper;
using CsvHelper.Configuration;
using GlideBuy.Core.Domain.Seo;
using GlideBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace GlideBuy.Data.Seeding
{
	public class MainSeeder
	{
		private readonly StoreDbContext _context;
		private readonly ILogger<MainSeeder> _logger;

		private readonly CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			Delimiter = "|"
		};

		public MainSeeder(
			StoreDbContext context,
			ILogger<MainSeeder> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task SeedCategoriesAsync()
		{
			if (_context.Categories.Any())
			{
				return;
			}

			var path = Path.Combine(AppContext.BaseDirectory, "Seed", "categories.csv");

			if (!File.Exists(path))
			{
				_logger.LogWarning("Categories seed file does not exist");

				return;
			}

			using var reader = new StreamReader(path);
			using var csv = new CsvReader(reader, config);
			var records = csv.GetRecords<CategorySeedRow>().ToList();

			foreach (var row in records)
			{
				var category = new Category
				{
					Id = row.Id,
					Name = row.Name,
					ParentCategoryId = row.ParentCategoryId,
					IconName = row.Icon,
					DisplayOrder = row.DisplayOrder,
				};

				_context.Categories.Add(category);
				_logger.LogDebug($"Added seed category with ID {category.Id}");
			}

			_logger.LogDebug("Openning the database connection");
			await _context.Database.OpenConnectionAsync();
			try
			{
				await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Categories ON");
				await _context.SaveChangesAsync();
				await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Categories OFF");

				_logger.LogDebug("Categories were saved to the database");
			}
			finally
			{
				await _context.Database.CloseConnectionAsync();
			}
		}

		public async Task SeedUrlRecordsAsync()
		{
			if (_context.UrlRecords.Any())
			{
				return;
			}

			var path = Path.Combine(AppContext.BaseDirectory, "Seed", "url_records.csv");

			if (!File.Exists(path))
			{
				return;
			}

			using var reader = new StreamReader(path);
			using var csv = new CsvReader(reader, config);
			var records = csv.GetRecords<UrlRecordSeedRow>().ToList();

			foreach (var row in records)
			{
				var urlRecord = new UrlRecord
				{
					EntityId = row.EntityId,
					EntityName = row.EntityName,
					Slug = row.Slug,
					IsActive = row.IsActive,
				};

				_context.UrlRecords.Add(urlRecord);
			}

			await _context.SaveChangesAsync();
		}

		public async Task SeedProductsAsync()
		{
			if (_context.Products.Any())
			{
				return;
			}

			var path = Path.Combine(AppContext.BaseDirectory, "Seed", "products.csv");

			if (!File.Exists(path))
			{
				return;
			}

			using var reader = new StreamReader(path);
			using var csv = new CsvReader(reader, config);
			var records = csv.GetRecords<ProductSeedRow>().ToList();

			foreach (var row in records)
			{
				var product = new Product
				{
					Name = row.Name,
					CategoryId = row.CategoryId,
					ShortDescription = row.ShortDescription,
					Price = row.Price
				};

				_context.Products.Add(product);
			}

			await _context.SaveChangesAsync();
		}
	}

	public sealed class ProductSeedRow
	{
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public int CategoryId { get; set; }
		public int Price { get; set; }
		public bool Published { get; set; }
		public bool Deleted { get; set; }
	}

	public sealed class UrlRecordSeedRow
	{
		public int EntityId { get; set; }
		public string EntityName { get; set; }
		public string Slug { get; set; }
		public bool IsActive { get; set; }
		public int LanguageId { get; set; }
	}

	public sealed class CategorySeedRow
	{
		public int Id { get; set; }

		public int? ParentCategoryId { get; set; }

		public string Name { get; set; }

		public int DisplayOrder { get; set; } = 0;

		public string Icon { get; set; } = string.Empty;
	}
}
