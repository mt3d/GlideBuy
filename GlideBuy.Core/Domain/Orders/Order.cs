using GlideBuy.Core.Domain.Payment;
using GlideBuy.Core.Domain.Shipping;
using GlideBuy.Models;
using GlideBuy.Models.Common;

namespace GlideBuy.Core.Domain.Orders
{
	public class Order : BaseEntity, ISoftDeletable
	{
		public Guid OrderGuid { get; set; }

		public int CustomerId { get; set; }


		public int BillingAddressId { get; set; }

		public int? ShippingAddressId { get; set; }

		public int? PickupAddressId { get; set; }

		public bool PickupInStore { get; set; }


		public PaymentStatus PaymentStatus { get; set; }

		public OrderStatus OrderStatus { get; set; }

		public ShippingStatus ShippingStatus { get; set; }


		public string PaymentMethodSystemName { get; set; }

		public string CustomerCurrencyCode { get; set; }


		public decimal OrderSubtotalInclTax { get; set; }

		public decimal OrderSubtotalExclTax { get; set; }

		public decimal OrderSubtotalDiscountInclTax { get; set; }

		public decimal OrderSubtotalDiscountExclTax { get; set; }

		public decimal OrderShippingInclTax { get; set; }

		public decimal OrderShippingExclTax { get; set; }

		public decimal PaymentMethodAdditionalFeeInclTax { get; set; }

		public decimal PaymentMethodAttditionalFeeExclTax { get; set; }

		public decimal TaxRates { get; set; }

		public decimal OrderTax { get; set; }

		public decimal OrderTotal { get; set; }


		public string CustomerIp { get; set; }


		public bool AllowStoringCreditCardInfo { get; set; }

		public string CardType { get; set; }

		public string CardName { get; set; }

		public string CardNumber { get; set; }


		public DateTime? PaidDateUtc { get; set; }

		
		public string ShippingMethod { get; set; }


		public bool Deleted { get; set; }

		public DateTime CreatedOnUtc { get; set; }

		// Without prefix.
		public string CustomOrderNumber { get; set; }
	}
}
