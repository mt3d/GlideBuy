using GlideBuy.Core.Domain.Seo;
using GlideBuy.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Support.Mvc.Routing
{
	public static class UrlHelperExtensions
	{
		/// <summary>
		/// Creates a search engine friendly URL for an entity of type T (Category for instance).
		/// Instead of manually writing something like /some-category-slug, calculates
		/// the correct URL format based on the routing configuration (and the category's search
		/// engine name. The SeName is the slug stored in the database for that category.)
		/// 
		/// It creates a generic mechanism that converts an entity type parameter into its corresponding route. The constraint ISlugSupported ensures that the entity type supports having a slug for SEO friendly navigation. Internally, this method resolves INopUrlHelper and calls its RouteGenericUrlAsync method. This indirection exists so that the routing for products, categories, manufacturers, and other searchable entities can be centralized in a single place instead of duplicated across views.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="urlHelper"></param>
		/// <returns></returns>
		/// Originally: GenerateUrl
		//public static string GenerateSeoUrlForEntity<T>(
		//	this IUrlHelper urlHelper,
		//	object? values = null)
		//		where T : BaseEntity, ISlugSupported
		//{

		//}
	}
}
