using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models;
using OnlineStore.Infrastructure;
using OnlineStore.Services.ProductCatalog;

namespace OnlineStore.Pages
{
    public class CartModel : PageModel
    {
        private IProductService productService;

        public CartModel(IProductService productService, Cart cartService)
        {
            this.productService = productService;
            Cart = cartService;
        }

        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product? product = productService.GetProductById(productId);

            if (product != null)
            {
                Cart.AddItem(product, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

		public IActionResult OnPostRemove(long productId, string returnUrl)
		{
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }
	}
}
