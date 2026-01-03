using GlideBuy.Core.Domain.Seo;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlideBuy.Models
{
	public class Category : BaseEntity, ISlugSupported
	{
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string MetaTitle { get; set; } = string.Empty;

		public string MetaKeywords { get; set; } = string.Empty;

		public string MetaDescription { get; set; } = string.Empty;

		public int PictureId { get; set; }

		public int? ParentCategoryId { get; set; } = 0;

		public Category? ParentCategory { get; set; }

		public bool Published { get; set; }

		public bool Deleted { get; set; }

		public int DisplayOrder { get; set; }

		public string IconName { get; set; }

		public DateTime CreatedOnUtc { get; set; }

		public DateTime UpdatedOnUtc { get; set; }

		[Column(TypeName = "decimal(19,4)")]
		public decimal MinPrice { get; set; }

		[Column(TypeName = "decimal(19,4)")]
		public decimal MaxPrice { get; set; }
	}
}
