using GlideBuy.Core;
using GlideBuy.Core.Domain.Customers;
using GlideBuy.Models.Customer;

namespace GlideBuy.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {
        protected readonly IWorkContext _workContext;
        protected readonly CustomerSettings _customerSettings;

        public CustomerModelFactory(
            IWorkContext workContext,
            CustomerSettings customerSettings)
        {
            _workContext = workContext;
            _customerSettings = customerSettings;
        }

        public virtual async Task<RegisterModel> PrepareRegisterModelAsync(RegisterModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var customer = await _workContext.GetCurrentCustomerAsync();

            model.AcceptPrivacyPolicyEnabled = _customerSettings.AcceptPrivacyPolicyEnabled;

            return model;
        }

        public virtual async Task<RegisterResultModel> PrepareRegisterResultModelAsync(int resultId, string? returnUrl)
        {
            var resultText = (UserRegistrationType)resultId switch
            {
                UserRegistrationType.Disabled => "Registration not allowed. You can edit this in the admin area.",
                UserRegistrationType.EmailValidation => "Your registration has been successfully completed. You have just been sent an email containing activation instructions.",
                UserRegistrationType.AdminApproval => "Your account will be activated after approving by administrator.",
                UserRegistrationType.Standard => "Registration was successful.",
                _ => ""
            };

            var model = new RegisterResultModel
            {
                Result = resultText,
                ReturnUrl = returnUrl,
            };

            return model;
        }

        public virtual async Task<LoginModel> PrepareLoginModelAsync(bool? checkoutAsGuest)
        {
            var model = new LoginModel
            {

            };

            return model;
        }
    }
}
