using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Directory;
using GlideBuy.Models.Common;

namespace GlideBuy.Web.Factories
{
	public interface IAddressModelFactory
	{
		Task PrepareAddressModelAsync(
			AddressModel model,
			Address address,
			Func<Task<IList<Country>>>? loadCountries = null,
			bool prePopulateWithCustomerFields = false);
	}
}
