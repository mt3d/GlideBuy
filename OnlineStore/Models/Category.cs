using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models
{
	public class Category
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string MetaTitle { get; set; } = string.Empty;

		public string MetaKeywords { get; set; } = string.Empty;

		public string MetaDescription { get; set; } = string.Empty;

		public int PictureId { get; set; }

		public bool Published { get; set; }
		public bool Deleted { get; set; }

		public int DisplayOrder { get; set; }

		public DateTime CreatedOnUtc { get; set; }

		public DateTime UpdatedOnUtc { get; set; }

		[Column(TypeName = "decimal(19,4)")]
		public decimal MinPrice { get; set; }

		[Column(TypeName = "decimal(19,4)")]
		public decimal MaxPrice { get; set; }
	}
}
