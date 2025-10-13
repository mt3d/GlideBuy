namespace GlideBuy.Infrastructure
{
	// TODO: Add an interface for testing purposes
	public class RouteProvider
	{
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
		}
	}
}
