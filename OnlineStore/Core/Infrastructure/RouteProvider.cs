namespace GlideBuy.Core.Infrastructure
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
			builder.MapControllerRoute(name: "ShoppingCart",
				pattern: "cart",
				new { controller = "ShoppingCart", action = "Cart" });

			// Checkout pages

			builder.MapControllerRoute(name: "Checkout",
				pattern: $"checkout",
				new { controller = "Checkout", action = "Index" });

			builder.MapControllerRoute(name: "CheckoutOnePage",
				pattern: $"onepagecheckout",
				new { controller = "Checkout", action = "OnePageCheckout" });

			// The normal customer login page but with an option to checkout as a guest.
			builder.MapControllerRoute(name: "LoginOrCheckoutAsGuest",
				pattern: $"login/checkoutasguest",
				new { controller = "Customer", action = "Login", checkoutAsGuest = true });


			// Old routes from the book.

			builder.MapControllerRoute(name: "catpage",
				pattern: "{category}/Page{productPage:int}",
				defaults: new { Controller = "Home", action = "Index" });

			builder.MapControllerRoute(name: "page",
				"Page{productPage:int}",
				new { Controller = "Home", action = "Index", productPage = 1 });

			// Shows the first page of items from a specific category
			//builder.MapControllerRoute("category",
			//	"{category}",
			//	new { Controller = "Home", action = "Index", productPage = 1 });

			//builder.MapDefaultControllerRoute();
		}
	}
}
