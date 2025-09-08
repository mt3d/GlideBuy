using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Models
{
	public class Product
	{
		public long? ProductId { get; set; }

		[Required(ErrorMessage = "Please enter a product name")]
		public string Name { get; set; } = String.Empty;

		[Required(ErrorMessage = "Please enter a description")]
		public string Description { get; set; } = String.Empty;

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
		[Column(TypeName = "decimal(8, 2)")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Please specify a category")]
		public string Category { get; set; } = String.Empty;

		public bool MarkedAsNew { get; set; }
		public bool Deleted { get; set; }
		// public originalStore name & link (below Product Name)
		// byline

		// public averageCustomerReviews
		// public int number of ratings;
		// public [] images;

		// public SpecialProductOverview
		// PressureCooker (Brand, Capacity, Material, Color, FinishType, ProductDimensions, Wattage, Item Weight)b
	}
}
