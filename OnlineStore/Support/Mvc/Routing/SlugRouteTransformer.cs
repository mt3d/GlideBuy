using GlideBuy.Core.Domain.Seo;
using GlideBuy.Models;
using GlideBuy.Services.Seo;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GlideBuy.Support.Mvc.Routing
{
	// TODO: WIP. the class isn't complete yet.
	/**
	 * While RouteProvider (which we looked at earlier) defines the patterns
	 * (the rules), SlugRouteTransformer executes the the logic (the enforcement)
	 * for every single request.
	 * 
	 * It inherits from DynamicRouteValueTransformer, a standard ASP.NET Core base
	 * class designed specifically for "translating" dynamic URLs into Controller/Action
	 * pairs asynchronously.
	 */
	public class SlugRouteTransformer : DynamicRouteValueTransformer
	{
		private readonly IUrlRecordService _urlRecordService;

		public SlugRouteTransformer(IUrlRecordService urlRecordService)
		{
			_urlRecordService = urlRecordService;
		}

		/**
		 * The routing middleware calls it when a matching dynamic endpoint is found, typically one that specifies {SeName:slug}. The method starts by copying the route values and then ensuring that a SeName key exists. It queries the IUrlRecordService using the slug to retrieve the UrlRecord. If no record is found, the method returns unchanged values, allowing MVC to fall back to other routes. If a record exists, the method publishes a routing event that allows plugins to short circuit the process. If the event does not stop processing, the transformer proceeds to examine whether the request contains a catalog path such as a category or manufacturer slug preceding the product slug. If so, it delegates to TryProductCatalogRoutingAsync, a method that specifically handles URLs of the form /catalogSlug/productSlug, depending on the configured product URL structure. This method verifies that the catalog portion is correct for the product and language, validates that the slugs match the active records, and potentially issues 301 redirects via InternalRedirect when discrepancies are detected. If everything is correct, it sets the controller to Product, the action to ProductDetails, and injects route parameters such as product ID and catalog seName, thereby completing the routing transformation.
		 */
		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			// 1. Extract the "slug" (e.g., "apple-macbook") from the URL.
			if (!values.TryGetValue("SeName", out var slug))
			{
				return values;
			}

			// 2. Query the database.
			// TODO: How could it be not a url record?
			if (await _urlRecordService.GetBySlugAsync(slug?.ToString() ?? "") is not UrlRecord urlRecord)
			{
				return values;
			}

			// TODO: 3. Extensibility Check: Triggers an event so plugins can hijack the routing if needed.

			// 4. Try Complex Routing (e.g. /category/product)
			var catalogPath = values.TryGetValue("CatalogSeName", out var catalogPathValue)
				? catalogPathValue?.ToString()
				: string.Empty;

			// 5. Default Routing (e.g. /product)
			await SingleSlugRoutingAsync(httpContext, values, urlRecord, catalogPath);

			return values;
		}

		/// <summary>
		/// Transform the route values according to the URL record.
		/// </summary>
		/// <param name="httpContext"></param>
		/// <param name="values"></param>
		/// <param name="urlRecord"></param>
		/// <param name="catalogPath"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		private async Task SingleSlugRoutingAsync(HttpContext httpContext, RouteValueDictionary values, UrlRecord urlRecord, string? catalogPath)
		{
			// TODO: Check if the slug is active.
			var slug = urlRecord.Slug;
			Console.WriteLine(slug.ToString());

			switch (urlRecord.EntityName)
			{
				case var name when name.Equals(nameof(Product), StringComparison.InvariantCultureIgnoreCase):
					RouteToAction(values, "Product", "ProductDetails", slug, ("productid", urlRecord.EntityId));
					return;
				case var name when name.Equals(nameof(Category), StringComparison.InvariantCultureIgnoreCase):
					RouteToAction(values, "Catalog", "Category", slug, ("categoryid", urlRecord.EntityId));
					return;
			}
		}

		private void RouteToAction(RouteValueDictionary values, string controller, string action, string slug, params (string Key, object value)[] parameters)
		{
			values["controller"] = controller;
			values["action"] = action;
			values["SeName"] = slug;

			foreach (var (key, value) in parameters)
			{
				values[key] = value;
			}
		}
	}
}
