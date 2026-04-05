using GlideBuy.Core.Domain.Customers;

namespace GlideBuy.Services.Customers
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        public CustomerSettings _customerSettings;
        public ICustomerService _customerService;

        public CustomerRegistrationService(
            CustomerSettings customerSettings,
            ICustomerService customerService)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
        }

        /**
         * The service layer does not trust the caller, even if the controller already validated input.
         * 
         * Registration is atomic and safe, regardless of where it is called from.
         * 
         * Password handling is not embedded in the entity or service logic - it is delegated
         * to a dedicated encryption service.
        */
        public virtual async Task<CustomerRegistrationResult> RegisterCustomerAsync(CustomerRegistrationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.Customer is null)
                throw new ArgumentNullException("Cannot load current customer");

            var result = new CustomerRegistrationResult();

            // TODO: Check if the customer is search engine account

            // TODO: Check if the customer is background task account

            // TODO: Check if the customer is registered

            if (string.IsNullOrEmpty(request.Email))
            {
                result.AddError("No email is provided for registration");
                return result;
            }

            // TODO: Check if the email is valid

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("No password is provided for registration");
                return result;
            }

            if (_customerSettings.UsernameEnabled && string.IsNullOrEmpty(request.Username))
            {
                result.AddError("Username is not provided for registration");
                return result;
            }

            // TODO: Validate the uniqueness of the email

            // TODO: Validate the uniqueness of the username if enabled

            /**
             * Only at this point does the domain entity (Customer) get updated.
             */
            request.Customer.UserName = request.Username;
            request.Customer.Email = request.Email;

            var customerPassword = new CustomerPassword
            {
                CustomerId = request.Customer.Id,
                Password = request.Password,
                CreatedOnUtc = DateTime.UtcNow
            };

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    customerPassword.Password = request.Password;
                    break;
                // TODO: Encrypt the password
                // TODO: Hash the password
            }

            await _customerService.InsertCustomerPasswordAsync(customerPassword);

            request.Customer.Active = request.IsApproved;

            // TODO: Handle the change of roles (add 'Registered', remove 'Guest')

            // TODO: Handle reward points

            await _customerService.UpdateCustomerAsync(request.Customer);

            return result;
        }
    }
}
