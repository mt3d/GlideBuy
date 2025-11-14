using GlideBuy.Core.Domain.Common;

namespace GlideBuy.Services.Common
{
	public interface IAddressService
	{
		Task InsertAddressAsync(Address address);
	}
}
