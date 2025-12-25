using GlideBuy.Support.Mvc.Routing;

namespace GlideBuy.Core.Infrastructure
{
	public class GenericUrlRouteProvider
	{
		public void AddRoutes(IEndpointRouteBuilder builder)
		{
			string? lang = null;

			/**
			 * The first part of the code handles standard "technical" routes.
			 * These are the fallbacks for pages that don't need SEO-friendly names
			 * (like the Admin panel, specific API endpoints, or the Home page).
			 */
			if (!string.IsNullOrEmpty(lang))
			{
				builder.MapControllerRoute(
					name: "DefaultWithLanguageCode",
					pattern: $"{lang}/{{controller=Home}}/{{action=Index}}/{{id?}}");
			}

			builder.MapControllerRoute(
				name: "Default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			// TODO: Check if the database is installed.
			/**
			 * The system stores URL slugs (keywords) in the database (UrlRecord table).
			 * If the database isn't installed yet, the system cannot look up these URLs,
			 * so it stops registering the advanced routes below to prevent crashes.
			 */

			// TODO: Move to a static class
			var CatalogSeName = "CatalogSeName";
			var SeName = "SeName";

			// Matches: /en/category-slug/product-slug
			var genericCatalogPattern = $"{lang}/{{{CatalogSeName}}}/{{{SeName}}}";
			// Matches: /en/product-slug (or category-slug, blog-slug, etc.)
			var genericPattern = $"{lang}/{{{SeName}}}";

			/**
			 * What is MapDynamicControllerRoute? It tells ASP.NET Core: "I see a URL
			 * pattern like /some-text, but I don't know which Controller handles it.
			 * Please ask the SlugRouteTransformer class to figure it out."
			 */
			builder.MapDynamicControllerRoute<SlugRouteTransformer>(genericCatalogPattern);
			builder.MapDynamicControllerRoute<SlugRouteTransformer>(genericPattern);




			// Routes for outbound URL generation
			// Outbound routing never calls SlugRouteTransformer. It never touches
			// the database. It never tries to map slugs to entity IDs.

			// Catalog slug and product slug (e.g. '/category-seo-name/product-seo-name')
			builder.MapControllerRoute(
				name: "ProductCatalog",
				pattern: genericCatalogPattern,
				defaults: new { controller = "Product", action = "ProductDetails" });

			// Product slug (e.g. '/product-seo-name')
			builder.MapControllerRoute(
				name: "Product",
				pattern: genericPattern,
				defaults: new { controller = "Product", action = "ProductDetails" });

			builder.MapControllerRoute(
				name: "Category",
				pattern: genericPattern,
				defaults: new { controller = "Catalog", action = "Category" });
		}
	}
}
