using GlideBuy.Core.Domain.Common;
using GlideBuy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlideBuy.Web.Models.Common
{
	public class AddressModel : BaseEntity
	{
		public string FirstName { get; set; }
		
		public string LastName { get; set; }

		public string Email { get; set; }

		public bool CountryEnabled { get; set; }
		public int? CountryId { get; set; }
		public string CountryName { get; set; }

		public int? DefaultCountryId { get; set; }

		public bool CityEnabled { get; set; }
		public bool CityRequired { get; set; }
		public string City { get; set; }

		public bool StreetAddressEnabled { get; set; }
		public bool StreetAddressRequired { get; set; }
		public string Address1 { get; set; }

		public bool StreetAddress2Enabled { get; set; }
		public bool StreetAddress2Required { get; set; }
		public string Address2 { get; set; }

		public bool ZipCodeEnabled { get; set; }
		public bool ZipCodeRequired { get; set; }
		public string ZipCode { get; set; }

		public IList<SelectListItem> AvailableCountries { get; set; }

		public Address ToEntity()
		{
			var address = new Address();

			address.Id = Id;
			address.FirstName = FirstName;
			address.LastName = LastName;
			address.Email = Email;
			//address.Company = Company;
			address.CountryId = CountryId;

			return address;
		}
	}
}
