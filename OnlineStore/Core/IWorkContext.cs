using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Core
{
	public interface IWorkContext
	{
		Task<Customer> GetCurrentCustomerAsync();

		Task SetCurrentCustomerAsync(Customer customer);
	}
}
