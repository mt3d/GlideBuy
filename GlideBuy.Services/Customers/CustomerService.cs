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

			var customerRole = await _staticCacheManager.TryGetOrLoadAsync(key, async () => await query.FirstOrDefaultAsync());

			return customerRole;
		}

		public async Task<Customer?> GetCustomerByIdAsync(int id)
		{
			return await _customerRepository.GetByIdAsync(id, cache => default); // TODO: Use short term cache.
		}

		public async Task<Customer?> GetCustomerByGuidAsync(Guid customerGuid)
		{
			if (customerGuid == Guid.Empty)
			{
				return null;
			}

			// TODO: Use customer repo instead
			var query = _context.Customers.Where(c => c.CustomerGuid == customerGuid).OrderBy(c => c.Id);

			/**
			 * the reason ShortTermCacheManager is used instead of StaticCacheManager in
			 * this scenario has to do with data freshness, request-level guarantees, and
			 * avoidance of stale customer data. Customer entities are particularly
			 * sensitive and must remain accurate during the entire HTTP request lifecycle,
			 * especially when they are used for authentication, authorization, and checkout logic.
			 * 
			 * If you cache a customer object in a static/global cache, such as
			 * StaticCacheManager, that cached version might persist for minutes or hours
			 * depending on the cache settings.
			 * 
			 * Short-term cache avoids database round-trips within a single request only.
			 * This optimizes performance by preventing duplicated queries while guaranteeing
			 * fresh data for the next request.
			 */
			// TODO: Use short-term cache manager
			return await query.FirstOrDefaultAsync();
		}

		public async Task UpdateCustomerAsync(Customer customer)
		{

		}
	}
}
