using GlideBuy.Core.Domain.Common;
using GlideBuy.Data;

namespace GlideBuy.Services.Common
{
	public class AddressService : IAddressService
	{
		private readonly IDataRepository<Address> _addressRepository;

		public AddressService(IDataRepository<Address> addressRepository)
		{
			_addressRepository = addressRepository;
		}

		public async Task InsertAddressAsync(Address address)
		{
			ArgumentNullException.ThrowIfNull(address);

			address.CreateOnUtc = DateTime.UtcNow;

			if (address.CountryId == 0)
			{
				address.CountryId = null;
			}

			await _addressRepository.InsertAsync(address);
		}
	}
}
