using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Directory;
using GlideBuy.Web.Models.Common;

namespace GlideBuy.Web.Factories
{
	public class AddressModelFactory : IAddressModelFactory
	{
		private readonly AddressSettings _addressSettings;

		public AddressModelFactory(AddressSettings addressSettings)
		{
			_addressSettings = addressSettings;
		}

		public async Task PrepareAddressModelAsync(
			AddressModel model,
			Address address,
			Func<Task<IList<Country>>>? loadCountries = null,
			bool prePopulateWithCustomerFields = false)
		{
			ArgumentNullException.ThrowIfNull(model);
		}
	}
}
