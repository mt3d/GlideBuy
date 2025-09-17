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
		public string ShortDescription { get; set; } = String.Empty;
		public string LongDescription { get; set; } = String.Empty; // TODO: Handle validation

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
		[Column(TypeName = "decimal(8, 2)")]
		public decimal Price { get; set; }

		//[Required(ErrorMessage = "Please specify a category")]
		public Category? Category { get; set; }

		// public Manufacturer Manufacturer {get;set;}

		public bool MarkedAsNew { get; set; }

		public bool Deleted { get; set; }

		// new features

		public int ProductTypeId { get; set; }


		public string MetaTitle { get; set; } = string.Empty;

		public string MetaKeywords { get; set; } = string.Empty;

		public string MetaDescription { get; set; } = string.Empty;

		public bool AllowReviews { get; set; }

		public int ApprovedRatingSum { get; set; }
		public int NotApprovedRaingSum { get; set; }

		public string Sku { get; set; } = string.Empty;

		// Global Trade Item Number
		// includes UPC, EAN, JAN, and ISBN for books
		public string Gtin { get; set; } = string.Empty;

		public bool IsDownloadable { get; set; } // TODO: IsDigital?
		public int DownloadId { get; set; }
		public bool UnlimitedDownloads { get; set; }
		public int MaxDownloads { get; set; }

		public bool HasUserAgreement { get; set; }

		public string UserAgreementText { get; set; } = string.Empty;

		public DateTime CreatedOnUtc { get; set; }

		public DateTime UpdatedOnUtc { get; set; }


		// public originalStore name & link (below Product Name)
		// byline

		// public averageCustomerReviews
		// public int number of ratings;
		// public [] images;

		// public SpecialProductOverview
		// PressureCooker (Brand, Capacity, Material, Color, FinishType, ProductDimensions, Wattage, Item Weight)b
	}
}
