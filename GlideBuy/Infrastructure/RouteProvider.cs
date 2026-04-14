using Microsoft.AspNetCore.Routing;

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
            builder.MapControllerRoute(name: "areaRoute",
                pattern: $"{{area:exists}}/{{controller=Home}}/{{action=Index}}/{{id?}}");

            builder.MapControllerRoute(name: "Homepage",
                pattern: "",
                defaults: new { controller = "Home", action = "index" });

            // Add product to cart from a product list page or component. Called using AJAX.
            // TODO: Add min(0) constraint.
            builder.MapControllerRoute(name: "AddProductToCart-Catalog",
                pattern: $"addproducttocart/catalog/{{productId}}/{{cartTypeId}}/{{quantity}}",
                defaults: new { controller = "ShoppingCart", action = "AddProductToCart_Catalog" });

            builder.MapControllerRoute(name: "ShoppingCart",
                pattern: "cart",
                new { controller = "ShoppingCart", action = "Cart" });


            builder.MapControllerRoute(name: "Register",
                pattern: $"register",
                defaults: new { controller = "Customer", action = "Register" });

            builder.MapControllerRoute(name: "RegisterResult",
                pattern: $"registerresult/{{resultId}}",
                defaults: new { controller = "Customer", action = "RegisterResult" });

            builder.MapControllerRoute(name: "Login",
                pattern: $"login",
                defaults: new { controller = "Customer", action = "Login" });


            builder.MapControllerRoute(name: "CustomerInfo",
                pattern: $"customer/info",
                defaults: new { controller = "Customer", action = "Info" });

            builder.MapControllerRoute(name: "CustomerAddresses",
                pattern: $"",
                defaults: new { controller = "Customer", action = "Addresses" });

            builder.MapControllerRoute(name: "CustomerNotifications",
                pattern: $"",
                defaults: new { controller = "Customer", action = "Notifications" });

            // TODO: Handle orders (paged or not)
            builder.MapControllerRoute(name: "CustomerOrders",
                pattern: $"",
                defaults: new { controller = "Customer", action = "Orders" });

            builder.MapControllerRoute(name: "CustomerPaymentMethods",
                pattern: $"",
                defaults: new { controller = "Customer", action = "PaymentMethods" });

            builder.MapControllerRoute(name: "CustomerProductReviews",
                pattern: $"",
                defaults: new { controller = "Customer", action = "CustomerProductReviews" });

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


            builder.MapControllerRoute(name: "CheckoutDeliveryInformation",
                pattern: $"checkout/deliveryinfo",
                defaults: new { controller = "Checkout", action = "DeliveryInformation" });

            builder.MapControllerRoute(name: "CheckoutShippingMethod",
                pattern: $"checkout/shippingmethod",
                defaults: new { controller = "Checkout", action = "ShippingMethod" });

            builder.MapControllerRoute(name: "CheckoutShippingAddress",
                pattern: $"checkout/shippingaddress",
                defaults: new { controller = "Checkout", action = "ShippingAddress" });

            builder.MapControllerRoute(name: "CheckoutPaymentMethod",
                pattern: $"checkout/paymentmethod",
                defaults: new { controller = "Checkout", action = "PaymentMethod" });

            // For confirming the order and adding it to the database. Redirect to other routes.
            //builder.MapControllerRoute(name: "CheckoutConfirm",
            //	pattern: $"checkout/confirm",
            //	defaults: new { controller = "Checkout", action = "Confirm" });

            // For displaying the "Thank you page" at the end of checkout.
            builder.MapControllerRoute(name: "CheckoutCompleted",
                pattern: $"checkout/completed/{{orderId:int?}}",
                defaults: new { controller = "Checkout", action = "Completed" });

            //builder.MapDefaultControllerRoute();
        }
    }
}
