using GlideBuy.Models;

namespace GlideBuy.Core.Domain.Seo
{
	/// <summary>
	/// Represents a URL record for SEO purposes (friendly names, or 'slugs').
	/// This entity maps a unique slug to a specific entity (like a Product or Category).
	/// </summary>
	public class UrlRecord : BaseEntity
	{
		/// <summary>
		/// Gets or sets the identifier of the entity this slug belongs to.
		/// This is the foreign key to the entity's primary table (e.g., the Product.Id).
		/// </summary>
		public int EntityId { get; set; }

		/// <summary>
		/// Gets or sets the name of the entity type (e.g., "Product", "Category", "Manufacturer").
		/// This is used by the SlugRouteTransformer to determine which controller to execute.
		/// </summary>
		public string EntityName { get; set; }

		/// <summary>
		/// Gets or sets the URL slug or friendly name (e.g., "apple-macbook-pro").
		/// This is the actual value that appears in the address bar.
		/// </summary>
		public string Slug { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this is the currently active/canonical slug.
		/// If an old slug exists, it is marked as inactive and used to generate a 301 redirect
		/// to the active slug.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the language identifier this slug is associated with.
		/// This supports multilingual SEO, allowing the same entity to have different
		/// slugs in different languages (e.g., "/en/book" vs. "/es/libro").
		/// </summary>
		public int LanguageId { get; set; }
	}
}
