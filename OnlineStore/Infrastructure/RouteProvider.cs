namespace GlideBuy.Infrastructure
{
	// TODO: Add an interface for testing purposes
	public class RouteProvider
	{
		/**
		 * The default route pattern usually produce less-user friendly URLs
		 * than the ones defined here.
		 * 
		 * So /cart instead of /ShoppingCart/Cart
		 */
		public void AddRoutes(IEndpointRouteBuilder builder)
		{
			builder.MapControllerRoute(name: "catpage",
				pattern: "{category}/Page{productPage:int}",
				defaults: new { Controller = "Home", action = "Index" });

			builder.MapControllerRoute(name: "page",
				"Page{productPage:int}",
				new { Controller = "Home", action = "Index", productPage = 1 });

			builder.MapControllerRoute("category",
				"{category}",
				new { Controller = "Home", action = "Index", productPage = 1 });

			builder.MapControllerRoute("pagination",
				"Products/Page{productPage}",
				new { Controller = "Home", action = "Index" });

			builder.MapControllerRoute(name: "ShoppingCart",
				pattern: "cart/",
				defaults: new { Controller = "ShoppingCart", action = "Cart" });
		}
	}
}
