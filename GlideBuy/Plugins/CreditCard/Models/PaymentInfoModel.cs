using GlideBuy.Support.Models;
using System.ComponentModel;

namespace GlideBuy.Plugins.CreditCard.Models
{
	public record PaymentInfoModel : BaseModel
	{
		[DisplayName("Card number")]
		public string CardNumber { get; set; }

		public string ExpireMonth { get; set; }

		public string ExpireYear { get; set; }

		// The CVV/CVC code (Card Verification Value/Code) is located on the back
		// of your credit/debit card on the right side of the white signature strip.
		public string CVC { get; set; }
	}
}
