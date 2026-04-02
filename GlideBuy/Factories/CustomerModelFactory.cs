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
    }
}
