using GlideBuy.Core.Domain.Orders;
using GlideBuy.Services.Orders;
using GlideBuy.Web.Models.ShoppingCart;
using GlideBuy.Services.Catalog;
using GlideBuy.Core.Domain.Common;

namespace GlideBuy.Web.Factories
{
	public class ShoppingCartModelsFactory : IShoppingCartModelsFactory
	{
		private readonly IProductService productService;
		private readonly OrderSettings _orderSettings;
		private readonly CommonSettings _commonSettings;
		private readonly IOrderProcessingService orderProcessingService;

		public ShoppingCartModelsFactory(
			IProductService productService,
			OrderSettings orderSettings,
			CommonSettings commonSettings,
			IOrderProcessingService orderProcessingService)
		{
			this.productService = productService;
			_orderSettings = orderSettings;
			_commonSettings = commonSettings;
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
				ProductId = item.Product.Id,
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

			model.HasTermsOfServiceOnCartPage = _orderSettings.HasTermsOfServiceOnCartPage;
			model.HasTermsOfServiceOnOrderConfirmPage = _orderSettings.HasTermsOfServiceOnOrderConfirmPage;
			model.HasTermsOfServicePopup = _commonSettings.PopupForTermsOfServiceLinks;

			foreach (var shoppingCartItem in cart)
			{
				var shoppingCartItemModel = await PrepareShoppingCartItemModelAsync(cart, shoppingCartItem);
				model.Items.Add(shoppingCartItemModel);
			}

			return model;
		}

		public async Task<OrderTotalsModel> PrepareOrderTotalsModelAsync(
			IList<ShoppingCartItem> cart,
			bool isEditable)
		{
			var model = new OrderTotalsModel { IsEditable = isEditable };

			if (!cart.Any())
			{
				return model;
			}

			// 1. Subtotal (TODO: Should be without discounts)
			model.NumberOfItems = cart.Count;
			model.Subtotal = cart.Sum(e => e.Product.Price * e.Quantity).ToString();

			// 2. Shipping Info

			// 3. Payment method fee

			// 4. Tax

			// 5. Total (TODO: Also with discounts)
			var totalDecimal = cart.Sum(e => e.Product.Price * e.Quantity);
			// TODO: Use price formatter
			model.Total = totalDecimal.ToString("c");

			// 6. Discount

			// 7. Gift cards

			// 8. Reward points spent

			// 9. Reward points earned

			return model;
		}

		public async Task<OrderSummaryModel> PrepareOrderSummaryModelAsync(bool isCartPage)
		{
			var model = new OrderSummaryModel();
			model.IsCartPage = isCartPage;

			return model;
		}
	}
}
