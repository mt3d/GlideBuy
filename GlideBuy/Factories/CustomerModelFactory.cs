using GlideBuy.Models.Customer;

namespace GlideBuy.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {
        public virtual async Task<RegisterModel> PrepareRegisterModelAsync(RegisterModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            return model;
        }
    }
}
