using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Data;

namespace GlideBuy.Services.Customers
{
	public class CustomerService : ICustomerService
	{
		private readonly StoreDbContext _context;

		public CustomerService(StoreDbContext context)
		{
			_context = context;
		}

		public async Task InsertCustomerAddressAsync(Customer customer, Address address)
		{
			ArgumentNullException.ThrowIfNull(customer);
			ArgumentNullException.ThrowIfNull(address);

			customer.ShippingAddress = address;
			await _context.SaveChangesAsync();
		}
	}
}
