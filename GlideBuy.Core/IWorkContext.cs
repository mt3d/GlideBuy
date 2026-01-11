using GlideBuy.Core.Domain.Customers;
using GlideBuy.Core.Domain.Directory;

namespace GlideBuy.Core
{
	public interface IWorkContext
	{
		Task<Customer> GetCurrentCustomerAsync();

		Task SetCurrentCustomerAsync(Customer? customer = null);

		Task<Currency> GetWorkingCurrencyAsync();
	}
}
