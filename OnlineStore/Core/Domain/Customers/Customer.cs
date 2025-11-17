using GlideBuy.Models;
using GlideBuy.Models.Common;
using GlideBuy.Models.Localization;

namespace GlideBuy.Core.Domain.Customers
{
	public class Customer : BaseEntity, ISoftDeletable
	{
		public Customer()
		{
			CustomerGuid = Guid.NewGuid();
		}

		public Guid CustomerGuid { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Gender { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public string Company { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public string ZipCode { get; set; }

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public int? CountryId { get; set; }

		public string Phone { get; set; }

		public string Fax { get; set; }

		public string VatNumber { get; set; }

		// TODO: Add VAT number status

		// TODO: Add time zone Id

		// TODO: Add currency Id

		public int? LanguageId { get; set; }

		public Language Language { get; set; }

		public string AdminComment { get; set; }

		public bool IsTaxExempt { get; set; }

		// TODO: Add affiliate and vendor Ids

		public bool RequireReLogin { get; set; }

		public bool Active { get; set; }

		public bool Deleted { get; set; }

		public string SystemName { get; set; }

		public string LastIpAddress { get; set; }

		public DateTime CreateOnUtc { get; set; }

		public DateTime? LastLoginDateUtc { get; set; }

		public DateTime LastActivityDateUtc { get; set; }

		public int? BillingAddressId { get; set; }

		public int? ShippingAddressId { get; set; }

		public bool MustChangePassword { get; set; }
	}
}
