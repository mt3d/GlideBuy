using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task SignInAsync(Customer customer, bool isPersistent);
    }
}
