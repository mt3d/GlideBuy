using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Services.Installation
{
    public partial class InstallationService
    {
        protected virtual async Task InstallCustomersAndUsersAsync()
        {
            // TODO: Replace values of SystemName with data to CustomerDefaults

            var crAdminstrators = new CustomerRole
            {
                Name = "Adminstrators",
                Active = true,
                IsSystemRole = true,
                SystemName = "Adminstrators"
            };
            var crRegistered = new CustomerRole
            {
                Name = "Registered",
                Active = true,
                IsSystemRole = true,
                SystemName = "Registered"
            };
            var crGuests = new CustomerRole
            {
                Name = "Guests",
                Active = true,
                IsSystemRole = true,
                SystemName = "Guests"
            };
            var crVendors = new CustomerRole
            {
                Name = "Vendors",
                Active = true,
                IsSystemRole = true,
                SystemName = "Vendors"
            };

            var customerRoles = new List<CustomerRole>
            {
                crRegistered,
                crGuests,
                crVendors
            };

            _dbContext.CustomerRoles.AddRange(customerRoles);
            await _dbContext.SaveChangesAsync();


            // TODO: Add the default store


            // Default admin user

            var adminUser = new Customer
            {
                // TODO: Replace the following with data from InstallationSettings
                CustomerGuid = Guid.NewGuid(),
                Email = "admin@glidebuy.com",
                UserName = "admin@glidebuy.com",
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                // Set store ID
            };

            // TODO: Set the default admin address
            // TODO: Set the default billing and shipping address

            _dbContext.Customers.Add(adminUser);
            await _dbContext.SaveChangesAsync();

            _dbContext.CustomerCustomerRoleMappings.Add(new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crAdminstrators.Id });
            _dbContext.CustomerCustomerRoleMappings.Add(new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crRegistered.Id });
            await _dbContext.SaveChangesAsync();

            // TODO: Add a user for search engine requests

            // TODO: Add a user for background tasks
        }
    }
}
