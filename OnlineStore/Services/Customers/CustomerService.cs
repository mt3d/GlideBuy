using GlideBuy.Core.Caching;
using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Data;
using Microsoft.EntityFrameworkCore;

namespace GlideBuy.Services.Customers
{
	public class CustomerService : ICustomerService
	{
		private readonly StoreDbContext _context;
		private readonly IStaticCacheManager _staticCacheManager;
		private readonly IDataRepository<Customer> _customerRepository;

		public CustomerService(
			StoreDbContext context,
			IStaticCacheManager staticCacheManager,
			IDataRepository<Customer> dataRepository)
		{
			_context = context;
			_staticCacheManager = staticCacheManager;
			_customerRepository = dataRepository;
		}

		public async Task InsertCustomerAddressAsync(Customer customer, Address address)
		{
			ArgumentNullException.ThrowIfNull(customer);
			ArgumentNullException.ThrowIfNull(address);

			customer.ShippingAddress = address;
			await _context.SaveChangesAsync();
		}

		public async Task<Customer> InsertGuestCustomerAsync()
		{
			var customer = new Customer()
			{
				CustomerGuid = Guid.NewGuid(),
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
			};

			// TODO: Move "Guests" to a defaults class.
			var guestRole = await GetCustomerRoleBySystemName("Guests") ?? throw new Exception("'Guests' role couldn't be loaded");

			customer.CustomerRole = guestRole;

			await _customerRepository.InsertAsync(customer);

			return customer;
		}

		public async Task<CustomerRole?> GetCustomerRoleBySystemName(string systemName)
		{
			if (string.IsNullOrWhiteSpace(systemName))
			{
				return null;
			}

			var key = _staticCacheManager.BuildKeyWithDefaultCacheTime(CustomerServiceDefaults.CustomerRolesBySystemNameCacheKey, systemName);

			var query = _context.CustomerRoles.OrderBy(r => r.Id).Where(r => r.SystemName == systemName);

			var customerRole = await _staticCacheManager.TryGetOrLoad(key, async () => await query.FirstOrDefaultAsync());

			return customerRole;
		}
	}
}
