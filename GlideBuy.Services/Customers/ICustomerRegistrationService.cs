using GlideBuy.Core.Domain.Customers;
using Microsoft.AspNetCore.Mvc;

namespace GlideBuy.Services.Customers
{
    public interface ICustomerRegistrationService
    {
        Task<CustomerRegistrationResult> RegisterCustomerAsync(CustomerRegistrationRequest request);

        Task<IActionResult> SignInCustomerAsync(Customer customer, string returnUrl, bool isPersistent = false);
    }
}
