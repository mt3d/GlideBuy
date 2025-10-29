using GlideBuy.Core.Domain.Orders;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Models.ShoppingCart;
using OnlineStore.Models;
using OnlineStore.Services.ProductCatalog;

namespace GlideBuy.Web.Factories
{
	public class ShoppingCartModelFactory : IShoppingCartModelFactory
	{
		private readonly IProductService productService;
		private readonly OrderSettings orderSettings;
		private readonly IOrderProcessingService orderProcessingService;

		public ShoppingCartModelFactory(
			IProductService productService,
			IOrderProcessingService orderProcessingService)
		{
			this.productService = productService;
			this.orderProcessingService = orderProcessingService;
		}

		private async Task<ShoppingCartModel.ShoppingCartItemModel> PrepareShoppingCartItemModelAsync(
			IList<ShoppingCartItem> cart,
			ShoppingCartItem item)
		{
			ArgumentNullException.ThrowIfNull(cart);
			ArgumentNullException.ThrowIfNull(item);

			// var product = await productService.GetProductByIdAsync(item.Product.ProductId);

			var cartItemModel = new ShoppingCartModel.ShoppingCartItemModel
			{
				ProductId = item.Product.ProductId ?? 0,
				Sku = await productService.FormatSkuAsync(item.Product),
				// TODO: Localize name
				ProductName = item.Product.Name,
				Quantity = item.Quantity
				// Price
				// Subtotal
			};

			// TODO: Calculate unit price
			// A very complicated process
			cartItemModel.UnitPrice = item.Product.Price.ToString("c");
			cartItemModel.UnitPriceValue = item.Product.Price;

			// TODO: Calculate subtotal
			// A very complicated process
			cartItemModel.Subtotal = (item.Quantity * item.Product.Price).ToString("c");
			cartItemModel.SubtotalValue = item.Quantity * item.Product.Price;

			return cartItemModel;
		}

		public async Task<ShoppingCartModel> PrepareShoppingCartModelAsync(
			ShoppingCartModel model,
			IList<ShoppingCartItem> cart,
			bool isEditable = true,
			bool validateCheckoutAttributes = false,
			bool prepareAndDisplayOrderReviewData = false)
		{
			ArgumentNullException.ThrowIfNull(model);
			ArgumentNullException.ThrowIfNull(cart);

			// Handle empty shopping cart.

			if (!cart.Any())
			{
				return model;
			}

			model.IsEditable = isEditable;

			bool minOrderSubtotalAmountIsMet = await orderProcessingService.ValidateMinOrderSubtotalAmountAsync(cart);
			if (!minOrderSubtotalAmountIsMet)
			{
				// TODO get the min subtotal amount, convert it from primary currency to store currency

				model.MinOrderSubtotalWarning = $"The minimum order subtotal amount is ";
			}

			foreach (var shoppingCartItem in cart)
			{
				var shoppingCartItemModel = await PrepareShoppingCartItemModelAsync(cart, shoppingCartItem);
				model.Items.Add(shoppingCartItemModel);
			}

			return model;
		}
	}
}
