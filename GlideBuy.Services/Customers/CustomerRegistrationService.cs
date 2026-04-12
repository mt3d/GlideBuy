using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GlideBuy.Services.Customers
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        protected readonly CustomerSettings _customerSettings;
        protected readonly ICustomerService _customerService;
        protected readonly IWorkContext _workContext;
        protected readonly IUrlHelperFactory _urlHelperFactory;
        protected readonly IActionContextAccessor _actionContextAccessor;
        protected readonly IAuthenticationService _authenticationService;

        public CustomerRegistrationService(
            CustomerSettings customerSettings,
            ICustomerService customerService,
            IWorkContext workContext,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IAuthenticationService authenticationService)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _workContext = workContext;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _authenticationService = authenticationService;
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
            var registeredRole = await _customerService.GetCustomerRoleBySystemName("Registered") ?? throw new Exception("'Registered' role cannot be loaded.");
            await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping
            {
                CustomerId = request.Customer.Id,
                CustomerRoleId = registeredRole.Id,
            });

            if (await _customerService.IsGuestAsync(request.Customer))
            {
                var guestRole = await _customerService.GetCustomerRoleBySystemName("Guests");
                await _customerService.RemoveCustomerRoleMappingAsync(request.Customer, guestRole);
            }

            // TODO: Handle reward points

            await _customerService.UpdateCustomerAsync(request.Customer);

            return result;
        }

        public virtual async Task<IActionResult> SignInCustomerAsync(Customer customer, string returnUrl, bool isPersistent = false)
        {
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();

            if (currentCustomer.Id != customer.Id)
            {
                // TODO: Handle affiliate

                // TODO: Migrate the shopping cart

                await _workContext.SetCurrentCustomerAsync(customer);
            }

            // TODO: Authenticate using the AuthenticationService
            await _authenticationService.SignInAsync(customer, isPersistent);

            // TODO: Raise an event

            // TODO: Log a successful login in the activity log

            // TODO: Handle return URL
            // Only redirect to local URLs
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

            if (!string.IsNullOrEmpty(returnUrl) && urlHelper.IsLocalUrl(returnUrl))
                return new RedirectResult(returnUrl);

            return new RedirectToRouteResult("Homepage", null);
        }
    }
}
