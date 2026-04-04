using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Services.Customers
{
    public class CustomerRegistrationRequest
    {
        public CustomerRegistrationRequest() { }

        public Customer Customer { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public PasswordFormat PasswordFormat { get; set; }

        public int StoreId { get; set; }

        public bool IsApproved { get; set; }
    }
}
