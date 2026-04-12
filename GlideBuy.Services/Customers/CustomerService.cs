using GlideBuy.Core;
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
        private readonly IDataRepository<CustomerCustomerRoleMapping> _customerCustomerRoleMappingRepository;
        private readonly IDataRepository<CustomerPassword> _customerPasswordRepository;
        private readonly IDataRepository<CustomerRole> _customerRoleRepository;

        public CustomerService(
            StoreDbContext context,
            IStaticCacheManager staticCacheManager,
            IDataRepository<Customer> customerRepository,
            IDataRepository<CustomerCustomerRoleMapping> customerCustomerRoleRepository,
            IDataRepository<CustomerPassword> customerPasswordRepository,
            IDataRepository<CustomerRole> customerRoleRepository)
        {
            _context = context;
            _staticCacheManager = staticCacheManager;
            _customerRepository = customerRepository;
            _customerCustomerRoleMappingRepository = customerCustomerRoleRepository;
            _customerPasswordRepository = customerPasswordRepository;
            _customerRoleRepository = customerRoleRepository;
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

            //customer.CustomerRole = guestRole;

            await _customerRepository.InsertAsync(customer);
            await _customerCustomerRoleMappingRepository.InsertAsync(new CustomerCustomerRoleMapping { CustomerId = customer.Id, CustomerRoleId = guestRole.Id });

            return customer;
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

        public virtual async Task<IPagedList<Customer>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllPagedAsync();

            return customers;
        }

        public virtual async Task InsertCustomerPasswordAsync(CustomerPassword customerPassword)
        {
            await _customerPasswordRepository.InsertAsync(customerPassword);
        }

        #region Customer roles

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

        protected virtual async Task<IDictionary<int, CustomerRole>> GetAllCustomerRolesDictionaryAsync()
        {
            return await _staticCacheManager.TryGetOrLoadAsync(_staticCacheManager.BuildKeyWithDefaultCacheTime(EntityCachingDefaults<CustomerRole>.AllCacheKey), async () =>
            {
                return await _customerRoleRepository.Table.ToDictionaryAsync(cr => cr.Id);
            });
        }

        public virtual async Task<bool> IsRegisteredAsync(Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return await IsInCustomerRoleAsync(customer, "Registered", onlyActiveCustomerRoles);
        }

        public virtual async Task<bool> IsInCustomerRoleAsync(Customer customer, string customerRoleName, bool onlyActiveCustomerRoles)
        {
            ArgumentNullException.ThrowIfNull(customer);
            ArgumentException.ThrowIfNullOrEmpty(customerRoleName);

            var customerRoles = await GetCustomerRolesAsync(customer, onlyActiveCustomerRoles);

            return customerRoles?.Any(cr => cr.SystemName == customerRoleName) ?? false;
        }

        public virtual async Task<IList<CustomerRole?>> GetCustomerRolesAsync(Customer customer, bool showHidden)
        {
            ArgumentNullException.ThrowIfNull(customer);

            var allRolesById = await GetAllCustomerRolesDictionaryAsync();

            // TODO: Use the short term cache manager

            // Get all mappings between the current customer and the roles
            var mappings = await _customerCustomerRoleMappingRepository.GetAllAsync(query => query.Where(crm => crm.CustomerId == customer.Id));

            // Use the ids to get the roles from the dictionary
            return mappings.Select(mapping => allRolesById.TryGetValue(mapping.CustomerRoleId, out var role) ? role : null).Where(cr => cr != null && (showHidden || cr.Active)).ToList();
        }

        public virtual async Task AddCustomerRoleMappingAsync(CustomerCustomerRoleMapping mapping)
        {
            await _customerCustomerRoleMappingRepository.InsertAsync(mapping);
        }

        public virtual async Task<bool> IsGuestAsync(Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return await IsInCustomerRoleAsync(customer, "Guests", onlyActiveCustomerRoles);
        }

        public virtual async Task RemoveCustomerRoleMappingAsync(Customer customer, CustomerRole role)
        {
            ArgumentNullException.ThrowIfNull(customer);
            ArgumentNullException.ThrowIfNull(role);

            var mapping = await _customerCustomerRoleMappingRepository.Table
                .SingleOrDefaultAsync(m => m.CustomerId == customer.Id && m.CustomerRoleId == role.Id);

            if (mapping is not null)
            {
                await _customerCustomerRoleMappingRepository.DeleteAsync(mapping);
            }
        }

        #endregion
    }
}
