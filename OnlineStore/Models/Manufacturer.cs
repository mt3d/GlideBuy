using GlideBuy.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlideBuy.Models
{
	public class Manufacturer : ISoftDeletable
	{
		public int ManufacturerId { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string MetaKeywords { get; set; } = string.Empty;

		public string MetaDescription { get; set; } = string.Empty;

		public string MetaTitle { get; set; } = string.Empty;

		public int PictureId { get; set; }

		public int PageSize { get; set; }

		public bool Published { get; set; }

		public bool Deleted { get; set; }

		public bool DisplayOrder { get; set; }

		public DateTime CreatedOnUtc { get; set; }

		public DateTime UpdatedOnUtc { get; set; }

		[Column(TypeName = "decimal(19,4)")]
		public decimal PriceMin { get; set; }

		[Column(TypeName = "decimal(19,4)")]
		public decimal PriceMax { get; set; }

		// TODO:
		// public int ManufacturerTemplateId { get; set; }
		// public bool AllowCustomersToSelectPageSize { get; set; }
		// public string PageSizeOptions { get; set; }
		// public bool SubjectToAcl { get; set; }
		// public bool PriceRangeFiltering { get; set; }
		// public bool ManuallyPriceRange { get; set; }
	}
}
