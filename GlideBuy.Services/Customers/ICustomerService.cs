using GlideBuy.Core;
using GlideBuy.Core.Domain.Common;
using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Services.Customers
{
    public interface ICustomerService
    {
        Task<CustomerRole?> GetCustomerRoleBySystemName(string systemName);

        Task InsertCustomerAddressAsync(Customer customer, Address address);

        Task<Customer> InsertGuestCustomerAsync();

        Task<Customer?> GetCustomerByIdAsync(int id);

        Task<Customer?> GetCustomerByGuidAsync(Guid customerGuid);

        Task UpdateCustomerAsync(Customer customer);

        Task<IPagedList<Customer>> GetAllCustomersAsync();

        Task InsertCustomerPasswordAsync(CustomerPassword customerPassword);

        Task<bool> IsRegisteredAsync(Customer customer, bool onlyActiveCustomerRoles = true);

        Task<bool> IsInCustomerRoleAsync(Customer customer, string customerRoleName, bool onlyActiveCustomerRoles);

        Task<IList<CustomerRole?>> GetCustomerRolesAsync(Customer customer, bool showHidden);

        Task AddCustomerRoleMappingAsync(CustomerCustomerRoleMapping mapping);

        Task<bool> IsGuestAsync(Customer customer, bool onlyActiveCustomerRoles = true);

        Task RemoveCustomerRoleMappingAsync(Customer customer, CustomerRole role);
    }
}
