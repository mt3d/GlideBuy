using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Support
{
	public class WebWorkContext : IWorkContext
	{
		public Task<Customer> GetCurrentCustomerAsync()
		{
			throw new NotImplementedException();
		}

		public Task SetCurrentCustomerAsync(Customer customer)
		{
			throw new NotImplementedException();
		}
	}
}
