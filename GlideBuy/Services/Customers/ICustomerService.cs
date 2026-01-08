using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Services.Customers
{
	public interface ICustomerService
	{
		Task InsertCustomerAddressAsync(Customer customer, Address address);

		Task<Customer> InsertGuestCustomerAsync();

		Task<Customer?> GetCustomerByGuidAsync(Guid customerGuid);

		Task UpdateCustomerAsync(Customer customer);
	}
}
