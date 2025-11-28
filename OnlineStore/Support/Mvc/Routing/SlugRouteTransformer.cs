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
		private readonly UrlRecordService _urlRecordService;

		public SlugRouteTransformer(UrlRecordService urlRecordService)
		{
			_urlRecordService = urlRecordService;
		}

		public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
		{
			// 1. Extract the "slug" (e.g., "apple-macbook") from the URL.
			if (!values.TryGetValue("SeName", out var slug))
			{
				return values;
			}

			// 2. Query the database.
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

			switch (urlRecord.EntityName)
			{
				case var name when name.Equals(nameof(Product), StringComparison.InvariantCultureIgnoreCase):
					RouteToAction(values, "Product", "ProductDetails", slug, ("productid", urlRecord.EntityId));
					return;
			}
		}

		private void RouteToAction(RouteValueDictionary values, string controller, string action, string slug, params (string Key, object value)[] parameters)
		{
			values["controller"] = controller;
			values["action"] = action;
			values["SeName"] = slug;
		}
	}
}
