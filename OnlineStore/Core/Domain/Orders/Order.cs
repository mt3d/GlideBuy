using Microsoft.AspNetCore.Mvc.ModelBinding;
using GlideBuy.Core.Domain.Payment;
using GlideBuy.Core.Domain.Shipping;
using GlideBuy.Models;
using System.ComponentModel.DataAnnotations;

namespace GlideBuy.Core.Domain.Orders
{
	public class Order
	{
		[BindNever]
		public int OrderId { get; set; }
		[BindNever]
		public ICollection<ShoppingCartItem> Lines { get; set; } = new List<ShoppingCartItem>();

		[Required(ErrorMessage = "Please enter a name")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Please enter the first address line")]
		public string? Line1 { get; set; }
		public string? Line2 { get; set; }
		public string? Line3 { get; set; }

		[Required(ErrorMessage = "Please enter a city name")]
		public string? City { get; set; }

		[Required(ErrorMessage = "Please enter a state name")]
		public string? State { get; set; }

		public string? Zip { get; set; }

		[Required(ErrorMessage = "Please enter a country name")]
		public string? Country { get; set; }
		public bool GiftWrap { get; set; }

		[BindNever]
		public bool Shipped { get; set; }

		/// <summary>
		/// Gets or sets the payment status
		/// </summary>
		public PaymentStatus PaymentStatus { get; set; }

		/// <summary>
		/// Gets or sets the order status
		/// </summary>
		public OrderStatus OrderStatus { get; set; }

		/// <summary>
		/// Gets or sets the shipping status
		/// </summary>
		public ShippingStatus ShippingStatus { get; set; }
	}
}
