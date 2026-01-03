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
			// Add product to cart from a product list page or component. Called using AJAX.
			// TODO: Add min(0) constraint.
			builder.MapControllerRoute(name: "AddProductToCart-Catalog",
				pattern: $"addproducttocart/catalog/{{productId}}/{{cartTypeId}}/{{quantity}}",
				defaults: new { controller = "ShoppingCart", action = "AddProductToCart_Catalog" });

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

			builder.MapControllerRoute(name: "CheckoutShippingMethod",
				pattern: $"/checkout/shippingmethod",
				defaults: new { controller = "Checkout", action = "ShippingMethod" });

			//builder.MapDefaultControllerRoute();
		}
	}
}
